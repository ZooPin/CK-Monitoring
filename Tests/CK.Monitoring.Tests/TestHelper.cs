using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CK.Core;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace CK.Monitoring.Tests
{

    static class TestHelper
    {
        static string _solutionFolder;
        
        static IActivityMonitor _monitor;
        static ActivityMonitorConsoleClient _console;

        static TestHelper()
        {
            _monitor = new ActivityMonitor();
            // Do not pollute the console by default...
            // ... but this may be useful sometimes: LogsToConsole does the job.
            _console = new ActivityMonitorConsoleClient();
        }

        public static IActivityMonitor ConsoleMonitor => _monitor; 

        public static bool LogsToConsole
        {
            get { return _monitor.Output.Clients.Contains( _console ); }
            set
            {
                if( value ) _monitor.Output.RegisterUniqueClient( c => c == _console, () => _console );
                else _monitor.Output.UnregisterClient( _console );
            }
        }

        public static string SolutionFolder
        {
            get
            {
                if( _solutionFolder == null ) InitalizePaths();
                return _solutionFolder;
            }
        }

        public static string CriticalErrorsFolder
        {
            get
            {
                if (_solutionFolder == null) InitalizePaths();
                return SystemActivityMonitor.RootLogPath + "CriticalErrors";
            }
        }

        public static List<StupidStringClient> ReadAllLogs( DirectoryInfo folder, bool recurse )
        {
            List<StupidStringClient> logs = new List<StupidStringClient>();
            ReplayLogs( folder, recurse, mon =>
            {
                var m = new ActivityMonitor( false );
                logs.Add( m.Output.RegisterClient( new StupidStringClient() ) );
                return m;
            }, TestHelper.ConsoleMonitor );
            return logs;
        }

        public static string[] WaitForCkmonFilesInDirectory( string directoryPath, int minFileCount )
        {
            string[] files;
            for( ; ; )
            {
                files = Directory.GetFiles( directoryPath, "*.ckmon", SearchOption.TopDirectoryOnly );
                if( files.Length >= minFileCount ) break;
                Thread.Sleep( 200 );
            }
            foreach( var f in files )
            {
                if( !FileUtil.CheckForWriteAccess( f, 3000 ) )
                {
                    throw new CKException( "CheckForWriteAccess exceeds 3000 milliseconds..." );
                }
            }
            return files;
        }

        public static void ReplayLogs( DirectoryInfo directory, bool recurse, Func<MultiLogReader.Monitor, ActivityMonitor> monitorProvider, IActivityMonitor m = null )
        {
            var reader = new MultiLogReader();
            using( m != null ? m.OpenTrace().Send( "Reading files from '{0}' {1}.", directory.FullName, recurse ? "(recursive)" : null ) : null )
            {
                var files = reader.Add( directory.EnumerateFiles( "*.ckmon", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly ).Select( f => f.FullName ) );
                if( files.Count == 0 )
                {
                    if( m != null ) m.Warn().Send( "No *.ckmon files found!" );
                }
                else
                {
                    var monitors = reader.GetActivityMap().Monitors;
                    if( m != null )
                    {
                        m.Trace().Send( String.Join( Environment.NewLine, files ) );
                        m.CloseGroup( String.Format( "Found {0} file(s) containing {1} monitor(s).", files.Count, monitors.Count ) );
                        m.OpenTrace().Send( "Extracting entries." );
                    }
                    foreach( var mon in monitors )
                    {
                        var replay = monitorProvider( mon );
                        if( replay == null )
                        {
                            if( m != null ) m.Info().Send( "Skipping activity from '{0}'.", mon.MonitorId );
                        }
                        else
                        {
                            mon.Replay( replay, m );
                        }
                    }
                }
            }
        }

        public static string PrepareLogFolder( string subfolder )
        {
            string p = SystemActivityMonitor.RootLogPath + subfolder;
            CleanupFolder( p );
            return p;
        }

        static void CleanupFolder( string folder )
        {
            int tryCount = 0;
            for( ; ; )
            {
                try
                {
                    if( Directory.Exists( folder ) ) Directory.Delete( folder, true );
                    Directory.CreateDirectory( folder );
                    File.WriteAllText( Path.Combine( folder, "TestWrite.txt" ), "Test write works." );
                    File.Delete( Path.Combine( folder, "TestWrite.txt" ) );
                    return;
                }
                catch( Exception ex )
                {
                    if( ++tryCount == 20 ) throw;
                    ConsoleMonitor.Info().Send( ex, "While cleaning up test directory. Retrying." );
                    System.Threading.Thread.Sleep( 100 );
                }
            }
        }

        static public void InitalizePaths()
        {
            if(_solutionFolder == null)
            {
                var projectFolder = GetProjectPath();
                _solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(projectFolder));
                SystemActivityMonitor.RootLogPath = Path.Combine(projectFolder, "RootLogPath");
                ConsoleMonitor.Info().Send($"SolutionFolder is: {_solutionFolder}\r\nRootLogPath is: {SystemActivityMonitor.RootLogPath}");
            }
            Assert.That( Directory.Exists(CriticalErrorsFolder) );
        }

        static string GetProjectPath([CallerFilePath]string path = null) => Path.GetDirectoryName(path);
        public static void DumpSampleLogs1(Random r, GrandOutput g)
        {
            var m = new ActivityMonitor(false);
            g.EnsureGrandOutputClient(m);
            m.SetTopic("First Activity...");
            if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
            using (m.OpenTrace().Send("Opening trace"))
            {
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Trace().Send("A trace in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Info().Send("An info in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Warn().Send("A warning in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Error().Send("An error in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Fatal().Send("A fatal in group.");
            }
            if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
            m.Trace().Send("End of first activity.");
        }

        public static void DumpSampleLogs2(Random r, GrandOutput g)
        {
            var m = new ActivityMonitor(false);
            g.EnsureGrandOutputClient(m);

            m.Fatal().Send(ThrowExceptionWithInner(false), "An error occured");
            m.SetTopic("This is a topic...");
            m.Trace().Send("a trace");
            m.Trace().Send("another one");
            m.SetTopic("Please, show this topic!");
            m.Trace().Send("Anotther trace.");
            using (m.OpenTrace().Send("A group trace."))
            {
                m.Trace().Send("A trace in group.");
                m.Info().Send("An info...");
                using (m.OpenInfo().Send(@"A group information... with a 
multi
-line
message. 
This MUST be correctly indented!"))
                {
                    m.Info().Send("Info in info group.");
                    m.Info().Send("Another info in info group.");
                    m.Error().Send(ThrowExceptionWithInner(true), "An error.");
                    m.Warn().Send("A warning.");
                    m.Trace().Send("Something must be said.");
                    m.CloseGroup("Everything is in place.");
                }
            }
            m.SetTopic(null);
            using (m.OpenTrace().Send("A group with multiple conclusions."))
            {
                using (m.OpenTrace().Send("A group with no conclusion."))
                {
                    m.Trace().Send("Something must be said.");
                }
                m.CloseGroup(new[] {
                        new ActivityLogGroupConclusion( "My very first conclusion." ),
                        new ActivityLogGroupConclusion( "My second conclusion." ),
                        new ActivityLogGroupConclusion( @"My very last conclusion
is a multi line one.
and this is fine!" )
                    });
            }
            m.Trace().Send("This is the final trace.");
        }

        public static Exception ThrowExceptionWithInner(bool loaderException = false)
        {
            Exception e;
            try { throw new Exception("Outer", loaderException ? ThrowLoaderException() : ThrowSimpleException("Inner")); }
            catch (Exception ex) { e = ex; }
            return e;
        }

        static Exception ThrowSimpleException(string message)
        {
            Exception e;
            try { throw new Exception(message); }
            catch (Exception ex) { e = ex; }
            return e;
        }
        static Exception ThrowLoaderException()
        {
            Exception e = null;
            try { Type.GetType("A.Type, An.Unexisting.Assembly", true); }
            catch (Exception ex) { e = ex; }
            return e;
        }
    }
}
