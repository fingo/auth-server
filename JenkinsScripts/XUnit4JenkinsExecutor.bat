xcopy "C:\Program Files\xunit\runner\*" "%~1xunit/*" /S /Y /I
call "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\AuthServer\XUnit4JenkinsExecutorCore.bat"

cd "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\JenkinsScripts"