# bankutskriftleser
Målet er å kunne ta kontoutskrifter i form av PDF fra bankene, og gjøre innholdet i dem enklere å bruke fra kode. (Mens vi venter på PSD2...).

Koden bruker på C# og .NET Core og inneholder parserene for de ulike konoutskriftene som er støttet (foreløig ganske få). Har planer om å 
lage programmer som benytter seg av de ekstraherte dataene etterhvert.

Programmer er avhengig av [Xpdf tools] (http://www.xpdfreader.com) - dvs pdftotext må kunne kjøres.

Programmet er utviklet på Windows, men fungerer også på Linux. Den skal fungerer så lenge avhengighetene
.NET Core (version >= 2.1) og Xpdf tools er installert.
