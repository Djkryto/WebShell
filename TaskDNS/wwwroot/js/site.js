
let historyCommand = document.getElementById("History");
let inputCommand = document.getElementById("inputCommand");

let setCountPress = false;
let countPress = 0;

let dataHistory;
let hubConnection;

const AddCommandAsync = async (dataTime, inputField, historyField) => {

    let urlAdd = "https://localhost:7032/Server/Add";
    let commandJS = { id: 0, data: dataTime, textCommand: inputField };

    historyField.value += inputField + "\n";
    /*site.SwitcStateLoader();*/

    if (inputField != "") {
        await fetch(urlAdd, {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(commandJS),
            headers: { 'Content-Type': 'application/json' }
        })
    }
}

const getHistoryAsync = async (historyField) => {

    let urlHistory = "https://localhost:7032/Server/History"
    let response = await fetch(urlHistory);
    let data = await response.json()

    dataHistory = data;

    for (let i in data) {
        historyField.value += data[i].textCommand + "\n"
    }

    DownScrolle();
}

const getOutputConsoleAsync = async (historyField) => {

    setTimeout(
        async function () {
            let urlOutput = "https://localhost:7032/Server/Output"
            let response = await fetch(urlOutput);
            let data = await response.json();

            historyField.value += data.textCommand;
        }
        , 100)
}

const connectToHub = async () => {

    const hub = new signalR.HubConnectionBuilder() //запуск подколючениz webSoket
        .withUrl("/chat")
        .build();

    hubConnection = hub;

    hub.on("Send", function (data) { //Вывод данных консоли
        historyCommand.value += data + "\n";
        DownScrolle();
       
    });

    hub.on("FlagInputClient", function (data) { //Вывод данных 
        if (data == false) {
            inputCommand.onkeypress = () => { return false };
            inputCommand.value = "";
        }
    });

    hub.start()
}

const cancelCommandAsync = async () => {
    let url = "https://localhost:7032/Server/Close_CMD"
    hubConnection = null;
    await fetch(url);
}

////StartCommandOnLoadPage////

getHistoryAsync(historyCommand) //вывод данных из БД

connectToHub();// Подключение к ChatHub

inputCommand.onkeydown = () => { return checkKey(event.key) }; //подгрузка истории введенных команд из БД

//    hubConnection.invoke("Send", message); отправка данных


document.getElementById("buttonCommand").onclick = async function Click() {
    var date = new Date();

    let dataTime = date.getDay() + "." + date.getMonth() + "." + date.getFullYear() +
        " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();

    let textInputCommand = inputCommand.value;

    AddCommandAsync(dataTime, textInputCommand, historyCommand);

    inputCommand.value = "";
}


function checkKey(key) {

    if (setCountPress == false) {
        countPress = dataHistory.length;
        setCountPress = true;
    }//Проверка что нажата клавиша первый раз для вывода последней в истории команд

    if (key == "ArrowUp") {
        countPress--;
        if (countPress < 0)
            countPress = 0

        inputCommand.value = dataHistory[countPress].textCommand;
    }
    else if (key == "ArrowDown") {
        countPress++;
        if (countPress > dataHistory.length - 1)
            countPress = dataHistory.length - 1

        inputCommand.value = dataHistory[countPress].textCommand;
    }
    if (key == "Enter") {
        AddCommandAsync("Data", inputCommand.value, historyCommand.value)
        historyCommand.value += inputCommand.value + "\n" + "\n";
        inputCommand.value = "";
    }

    if (key == "Control") { //Остановка команды
        hubConnection.stop();
        cancelCommandAsync();

        connectToHub();
    }

    if (key == "X" || key == "x") {
        const hubConnection = new signalR.HubConnectionBuilder() //запуск подколючения к webSoket
            .withUrl("/chat")
            .build();

        hubConnection.on("Send", function (data) { //Вывод данных 
            historyCommand.value += data + "\n";
            DownScrolle();
        });

        hubConnection.start();
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

function DownScrolle() {
    historyCommand.scrollTop = historyCommand.scrollHeight;
}