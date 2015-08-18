;--------------------------------

XPStyle on

; The name of the installer
Name "Install Awareness"

; The file to write
OutFile "InstallAwareness.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Awareness

; Registry key for installation directory
InstallDirRegKey HKCU "Software\Futureproof\Awareness" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page directory
Page instfiles

;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR

  WriteRegStr HKCU SOFTWARE\Futureproof\Awareness "Install_Dir" "$INSTDIR"
  
  File ..\Resources\bowl.wav
  File ..\Resources\bowl.ico
  File ..\Resources\bowl92.png
  File ..\Resources\splash.jpg
  File ..\Awareness.Windows\Awareness\bin\Release\Awareness.exe
  File ..\Awareness.Windows\Awareness\bin\Release\Awareness.exe.config
  File ..\Awareness.Windows\Awareness\bin\Release\Cadenza.dll
  File ..\Awareness.Windows\Awareness\bin\Release\Awareness.Agnostic.dll

  CreateShortcut "$SMPROGRAMS\Awareness.lnk" $INSTDIR\Awareness.exe
  CreateShortcut "$SMPROGRAMS\Startup\Awareness.lnk" $INSTDIR\Awareness.exe

SectionEnd ; end the section
