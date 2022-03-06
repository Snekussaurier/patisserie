// JavaScript Datei für die Serverraumüberwachung
//
// Die Uhrzeit wird jede Sekunde aktualisiert
// Alle 5 Sekunden werden die Daten vom Server angefordert und in die Webseite geschrieben

window.setInterval(setTime, 1000);
window.setInterval(getData, 5000);

// Aktualisiert jede Sekunde die Uhrzeit

function setTime()
{
  date = new Date();
  document.getElementById("clock").innerHTML = date.toLocaleTimeString();
}

// Holt die Daten vom Server

async function getData()
{
    data =  fetch('http://localhost:1111/data')
    .then(response => response.json())
    .then(data => writeToHTML(data));
}

// Schreibt die Daten vom Server in die Webseite

async function writeToHTML(jsondata)
{
    let data = JSON.parse(jsondata);
    document.getElementById("room").innerHTML = data.room;
    document.getElementById("temp").innerHTML = data.temp.toFixed(1);   //eine Nachkommastelle
    document.getElementById("humid").innerHTML = data.humid.toFixed(1);
    document.getElementById("tlimit").innerHTML = data.tlimit.toFixed(1);
    document.getElementById("hlimit").innerHTML = data.hlimit.toFixed(1);
}

// gleich am Anfang einmal Daten laden
window.onload = getData()