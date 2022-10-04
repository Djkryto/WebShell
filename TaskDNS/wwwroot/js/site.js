import * as server from './server.js'; 


let historyCommand = document.getElementById("History");
let inputCommand = document.getElementById("inputCommand");

let setCountPress = false;
let countPress = 0;

////StartCommandOnLoadPage////
server.getHistory(historyCommand)
inputCommand.onkeydown = () => { return checkKey(event.key) };
/////////////////////////////


document.getElementById("buttonCommand").onclick = async function Click() {
    var date = new Date();

    let dataTime = date.getDay() + "." + date.getMonth() + "." + date.getFullYear() +
        " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();

    let textInputCommand = inputCommand.value;
    inputCommand.value = "";

    server.AddCommand(dataTime, textInputCommand, historyCommand);
    setTimeout(server.getOutputConsole, 5000, historyCommand);// Задержка перед отправкой запросадля обратки команнды на сервере.
}

function checkKey(key) {

    if (setCountPress == false) {
        countPress = server.dataHistory.length;
        setCountPress = true;
    }//Проверка что нажата клавиша первый раз для вывода последней в истории команд

    if (key == "ArrowUp") {
        countPress--;
        if (countPress < 0)
            countPress = 0
        
        inputCommand.value = server.dataHistory[countPress].textCommand;
    }
    else if (key == "ArrowDown") {
        countPress++;
        if (countPress > server.dataHistory.length - 1)
            countPress = server.dataHistory.length - 1
        
        inputCommand.value = server.dataHistory[countPress].textCommand;
    }

    return key
}

function SwitcStateLoader() {
    let loader = document.getElementById("loading").classList;

    if (loader.contains("offLoading")) {
        loader.remove("offLoading");
    }
    else {
        loader.add("offLoading");
    }
}

export { SwitcStateLoader }