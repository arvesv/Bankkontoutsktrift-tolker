# bankutskriftleser
Målet er å kunne ta kontoutskrifter (i form av PDF) fra bankene, og gjøre innholdet i dem enklere å bruke fra kode. (mens vi venter på PSD2....).


Biblioteket Core er laget på .NET core og inneholder parserene for de ulike konoutskriftene som er støttet (foreløig ganske få). Har planer om å 
lage programmer som benytter seg av de ekstraherte dataene etterhvert.

Programmer er avhengig av [Xpdf tools] (http://www.xpdfreader.com) - dvs pdftotext må kunne kjøres. 

Programmet er utviklet på Windows, men både .NET CORE og Xpdf tools skal fungere på Linux - dvs fungerer kanskje.
