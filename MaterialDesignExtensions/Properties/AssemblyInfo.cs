﻿using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// Allgemeine Informationen über eine Assembly werden über die folgenden
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
/*[assembly: AssemblyTitle("Material Design Extensions")]
[assembly: AssemblyDescription("Material Design Extensions is based on Material Design in XAML Toolkit to provide additional controls and features for WPF apps")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Philipp Spiegel")]
[assembly: AssemblyProduct("Material Design Extensions")]
[assembly: AssemblyCopyright("Copyright © 2017-2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]*/

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten.  Wenn Sie auf einen Typ in dieser Assembly von 
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.
[assembly: ComVisible(false)]

//Um mit dem Erstellen lokalisierbarer Anwendungen zu beginnen, legen Sie
//<UICulture>ImCodeVerwendeteKultur</UICulture> in der .csproj-Datei
//in einer <PropertyGroup> fest.  Wenn Sie in den Quelldateien beispielsweise Deutsch
//(Deutschland) verwenden, legen Sie <UICulture> auf \"de-DE\" fest.  Heben Sie dann die Auskommentierung
//des nachstehenden NeutralResourceLanguage-Attributs auf.  Aktualisieren Sie "en-US" in der nachstehenden Zeile,
//sodass es mit der UICulture-Einstellung in der Projektdatei übereinstimmt.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //Speicherort der designspezifischen Ressourcenwörterbücher
                             //(wird verwendet, wenn eine Ressource auf der Seite nicht gefunden wird,
                             // oder in den Anwendungsressourcen-Wörterbüchern nicht gefunden werden kann.)
    ResourceDictionaryLocation.SourceAssembly //Speicherort des generischen Ressourcenwörterbuchs
                                      //(wird verwendet, wenn eine Ressource auf der Seite nicht gefunden wird,
                                      // designspezifischen Ressourcenwörterbuch nicht gefunden werden kann.)
)]


// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder Standardwerte für die Build- und Revisionsnummern verwenden,
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
/*[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]*/

[assembly: XmlnsPrefix("https://spiegelp.github.io/MaterialDesignExtensions/winfx/xaml", "mde")]
[assembly: XmlnsDefinition("https://spiegelp.github.io/MaterialDesignExtensions/winfx/xaml", "MaterialDesignExtensions.Controls")]
[assembly: XmlnsDefinition("https://spiegelp.github.io/MaterialDesignExtensions/winfx/xaml", "MaterialDesignExtensions.Converters")]
[assembly: XmlnsDefinition("https://spiegelp.github.io/MaterialDesignExtensions/winfx/xaml", "MaterialDesignExtensions.Model")]
