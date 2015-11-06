; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "wDialogu"
!define PRODUCT_VERSION "1.1"
!define PRODUCT_PUBLISHER "wDialogu"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\TeamTalkApp.NET.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; MUI 1.67 compatible ------
!include "MUI.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "favicon.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Language Selection Dialog Settings
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"



; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\TeamTalkApp.NET.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "Polish"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\wDialogu"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowUnInstDetails show

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

Section "Program" SEC01
  SetOutPath "$INSTDIR\logs"
  SetOverwrite try
  SetOutPath "$INSTDIR\sounds"
  SetOverwrite try
  File "resources\ding.wav"

  SetOutPath "$INSTDIR"
  File "bin\x86\UI Test\Newtonsoft.Json.dll"
  File "libraries\lame_enc.dll"
  File "libraries\NAudio.dll"
  File "libraries\NAudio.WindowsMediaFormat.dll"

  File "bin\x86\UI Test\NLog.dll"
  File "bin\x86\UI Test\TeamTalk4Pro.NET.dll"
  File "bin\x86\UI Test\TeamTalk5Pro.dll"
  File "bin\x86\UI Test\TeamTalkApp.NET.application"
  File "bin\x86\UI Test\TeamTalkApp.NET.exe"
  File "bin\x86\UI Test\TeamTalkLib.NET.dll"
  CreateDirectory "$SMPROGRAMS\wDialogu"
  CreateShortCut "$SMPROGRAMS\wDialogu\wDialogu.lnk" "$INSTDIR\TeamTalkApp.NET.exe"
  CreateShortCut "$DESKTOP\wDialogu.lnk" "$INSTDIR\TeamTalkApp.NET.exe"
  File "bin\x86\UI Test\TeamTalkApp.NET.exe.manifest"
  Exec '"$INSTDIR\TeamTalkApp.NET.exe" --register'
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\wDialogu\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\TeamTalkApp.NET.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\TeamTalkApp.NET.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\TeamTalkApp.NET.exe.manifest"
  Delete "$INSTDIR\TeamTalkApp.NET.exe"
  Delete "$INSTDIR\TeamTalkApp.NET.application"
  Delete "$INSTDIR\TeamTalk5Pro.dll"
  Delete "$INSTDIR\TeamTalk4Pro.NET.dll"
  Delete "$INSTDIR\Shared.dll"
  Delete "$INSTDIR\NLog.dll"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\logs\TeamTalk.log"

  Delete "$SMPROGRAMS\wDialogu\Uninstall.lnk"
  Delete "$DESKTOP\wDialogu.lnk"
  Delete "$SMPROGRAMS\wDialogu\wDialogu.lnk"

  RMDir "$SMPROGRAMS\wDialogu"
  RMDir "$INSTDIR\logs"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd