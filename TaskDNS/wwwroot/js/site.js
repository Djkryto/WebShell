
let historyCommand = document.getElementById("listOutputCommand");
let inputCommand = document.getElementById("inputCommand");
let allElement = document.getElementById("Console");

let setCountPress = false;
let countPress = 0;
let countChild = 0;

let hubConnection;
let dataHistory;

let childArrayPath;
let path = "C:\\";
let command = "";
let childPath;

const AddCommandAsync = async (dataTime, inputField) => {

    let urlAdd = "https://localhost:7032/Server/Add";
    let commandJS = { id: 0, data: dataTime, textCommand: inputField.replace('\"', '') };

    let li = document.createElement("li");
    li.append(inputField + "\n")

    historyCommand.appendChild(li); 
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

const postPathAsync = async (pathClient, inputField) => {

    let urlAdd = "https://localhost:7032/Server/Change_Path";
    let li = document.createElement("li");
    li.append(inputField)

    historyCommand.appendChild(li);
    if (inputField != "") {
        await fetch(urlAdd, {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(pathClient),
            headers: { 'Content-Type': 'application/json' }
        })
    }

    await getPathAsync();
}

const getPathAsync = async () => {
    let url = "https://localhost:7032/Server/Get_Path";
    let responce = await fetch(url);
    let data = await responce.text();

    path = data;
    DownScroll();
    inputCommand.value = path + "\>";
}

const getPathChildDirectoryAsync = async () => {
    let url = "https://localhost:7032/Server/Get_ArrayChildPath";
    let responce = await fetch(url);
    let data = await responce.json();

    childArrayPath = Object.keys(data).map(key => [data[key]])
}

const getHistoryAsync = async () => {

    let urlHistory = "https://localhost:7032/Server/History"
    let response = await fetch(urlHistory);
    let data = await response.json()

    dataHistory = data;

    for (let i in data) {
        let li = document.createElement("li");
        li.append( data[i].textCommand + "\n")

        historyCommand.appendChild(li);
        DownScroll();
    }

    getPathAsync();
    getPathChildDirectoryAsync();
}


const connectToHub = async () => {

    const hub = new signalR.HubConnectionBuilder() //запуск подколючениz webSoket
        .withUrl("/chat")
        .build();
    hubConnection = hub;
    hub.on("Send", function (data) { //Вывод данных консоли 
        getPathChildDirectoryAsync();
        WriteHistoryCommand(data.status, data.output);
        getPathAsync();
        DownScroll();
        if (data.status == 0) {
            OffInput();
        }
        else {
            OnInput();
        }
    });

    inputCommand.value = path + "\>"
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

inputCommand.focus();
//    hubConnection.invoke("Send", message); отправка данных


function checkKey(key) {

    if (setCountPress == false) {
        countPress = dataHistory.length;
        setCountPress = true;
    }//Проверка что нажата клавиша первый раз для вывода последней в истории команд

    if (key == "Backspace" || key == "ArrowLeft" || key == "A") {
        let lenghtInputCommand = inputCommand.value.length - 1;
        if (inputCommand.value[lenghtInputCommand] == ">") {
            return false;
        }
    }

    if (key == "ArrowUp") {
        countPress--;
        if (countPress < 0)
            countPress = 0

        inputCommand.value = path + "\> " + dataHistory[countPress].textCommand;
    }
    else if (key == "ArrowDown") {
        countPress++;
        if (countPress > dataHistory.length - 1)
            countPress = dataHistory.length - 1

        inputCommand.value = path + "\> " + dataHistory[countPress].textCommand;
    }
    if (key == "Enter") {

        historyCommand.value += inputCommand.value + "\n" + "\n";
        getCommand(inputCommand.value)
        AddCommandAsync("Data", command, historyCommand.value)
        inputCommand.value = path + "\> ";

        command = "";
    }

    if (key == "Control") { //Остановка команды
        hubConnection.stop();
        cancelCommandAsync();

        connectToHub();
    }

    if (key == "Tab")
        if (childArrayPath != null) {
            childPath = currentPath(path, childArrayPath[countChild]);

            inputCommand.value = path + "\>" + childPath;
            inputCommand.focus();
        }
    
    if (inputCommand.value == "") {
        inputCommand.value = path + "\>"
    }
    return key
}

function DownScroll() {
    //  historyCommand.scrollIntoView(false);
    inputCommand.scrollIntoView(false);
}

function OffInput() {
    inputCommand.onkeypress = () => { return false };
}

function OnInput() {
    inputCommand.onkeypress = () => { return true };
}

function WriteHistoryCommand(status, dataText) {

    if (status != 2) {

        let li = document.createElement("li");
        let pre = document.createElement("pre");

        pre.append(dataText);
        li.append(pre);

        if (status == 1) {
            li.classList.add("red");
        } else {
            li.classList.add("white");
        }

        historyCommand.appendChild(li);
        DownScroll();
    }
}


function getCommand(inputCommandText) {

    let isStartReadCommand;
    let isReadComman;
    let i = 0;

    while (i != inputCommandText.length) {
        if (inputCommandText[i] == ">") {
            isStartReadCommand = true;
            i++;
        }
        if (isStartReadCommand) {
            if (inputCommandText[i] != " ")
                isReadComman = true;
            if (isReadComman)
                command += inputCommandText[i];
        }
        i++;
    }
}

function currentPath(pathMain,pathArrayChild) {
    let lineReturn = "";

    let array = pathArrayChild[0];
    for (let i = 0; i < array.length; i++) {
        if (array[i] != pathMain[i]) {
            lineReturn += array[i];
        }
    }

    if (countChild == childArrayPath.length - 1)
        countChild = 0;
    else
        countChild++;

    lineReturn = lineReturn.slice(0);
    return lineReturn;
}
