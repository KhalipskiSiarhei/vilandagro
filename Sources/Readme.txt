If you add new project it is necessary to do the following things:
1) - Enable Code Analysis (project option page, Code Anallysis tab)
2) - Configure project to use StyleCop global settings file (StyleCop Settings project menu, Merge tab, set path to the settings in the root folder)
3) - Add to the *.sproj file the following line <Import Project="$(ProgramFiles)\MSBuild\StyleCop\v4.7\StyleCop.targets" />