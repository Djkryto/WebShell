
let historyCommand = document.getElementById("listOutputCommand");
let inputCommand = document.getElementById("inputCommand");
let allElement = document.getElementById("Console");

let setCountPress = false;
let countPress = 0;
let countChild = 0;

let hubConnection;
let dataHistory;

let dirictoriesArray;
let mainDirectory = "C:\\";
let command = '';
let currentDirictoryPath;

/**
   * JavaScript функция - имеет аннотацию с указанием типа
    * @param {number} dataTime текущее время.
    * @param {number} inputFieldText текс из InputField.
    */
const AddCommandAsync = async (dataTime, inputFieldText) => {
    //Отправка данных на сервер
    let urlAdd = "https://localhost:7032/Server/Add";
    let commandJS = { id: 0, data: dataTime, textCommand: inputFieldText.replace('\"', '') };

    let li = document.createElement('li');
    li.append(inputFieldText + "\n")

    historyCommand.appendChild(li); 

    if (inputFieldText != '') {
        await fetch(urlAdd, {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(commandJS),
            headers: { 'Content-Type': 'application/json' }
        })
    }
}
/**
   * JavaScript функция - без аннотации.
   */
const getDirectoryAsync = async () => {
    //Получиьть путь у сервера.
    let url = "https://localhost:7032/Server/Get_Directory";
    let responce = await fetch(url);
    let data = await responce.text();

    mainDirectory = data;
    DownScroll();
    inputCommand.value = mainDirectory + "\>";
}
/**
   * JavaScript функция - без аннотации.
   */
const getDirectoriesAsync = async () => {
    //Получить список директорий относительна текущей(главной) директории.
    let url = "https://localhost:7032/Server/Get_Directories";
    let responce = await fetch(url);
    let data = await responce.json();

    DirictoryesPathArray = Object.keys(data).map(key => [data[key]])
}

/**
   * JavaScript функция - без аннотации.
   */
const getHistoryAsync = async () => {
    //Получени истории из базы данных.
    let urlHistory = "https://localhost:7032/Server/Get_History"
    let response = await fetch(urlHistory);
    let data = await response.json()

    dataHistory = data;

    for (let i in data) {
        let li = document.createElement("li");
        li.append( data[i].textCommand + "\n")

        historyCommand.appendChild(li);
        DownScroll();
    }

    getDirectoryAsync();
    getDirectoriesAsync();
}

/**
   * JavaScript функция - без аннотации.
   */
const connectToHub = async () => {
    //Подключение к WebSoket.
    const hub = new signalR.HubConnectionBuilder() 
        .withUrl("/chat")
        .build();
    hubConnection = hub;
    hub.on("Send", function (data) {
        getDirectoriesAsync();
        WriteHistoryCommand(data.status, data.output);
        getDirectoryAsync();
        DownScroll();

        if (data.status == 0)
            OffInput();
        else
            OnInput();
    });

    inputCommand.value = mainDirectory + "\>"
    hub.start()
}

/**
   * JavaScript функция - имеет аннотацию с указанием типа.
   */
const cancelCommandAsync = async () => {
    //Послать на сервер запрос на отмену команды.
    let url = "https://localhost:7032/Server/Stop"
    hubConnection = null;
    await fetch(url);
}

////StartCommandOnLoadPage////

getHistoryAsync(historyCommand)
connectToHub();
inputCommand.onkeydown = () => { return checkKey(event.key) };
inputCommand.focus();

/**
   * JavaScript функция - имеет аннотацию с указанием типа.
    * @param {string} key является кодом нажатой кнопки.
   */
function checkKey(key) {
    
    if (setCountPress == false) {
        countPress = dataHistory.length;
        setCountPress = true;
    }

    if (key == "Backspace" || key == "ArrowLeft" || key == "A") {
        let lenghtInputCommand = inputCommand.value.length - 1;
        if (inputCommand.value[lenghtInputCommand] == ">")
            return false;
        
    }

    if (key == "ArrowUp") {
        countPress--;
        if (countPress < 0)
            countPress = 0

        inputCommand.value = mainDirectory + "\> " + dataHistory[countPress].textCommand;
    }
    else if (key == "ArrowDown") {
        countPress++;
        if (countPress > dataHistory.length - 1)
            countPress = dataHistory.length - 1

        inputCommand.value = mainDirectory + "\> " + dataHistory[countPress].textCommand;
    }
    if (key == "Enter") {

        historyCommand.value += inputCommand.value + "\n" + "\n";
        getCommand(inputCommand.value)
        AddCommandAsync("Data", command)
        inputCommand.value = mainDirectory + "\> ";

        command = '';
    }

    if (key == "Control") { //Остановка команды
        cancelCommandAsync();
    }

    if (key == "Tab") {
        if (DirictoryesPathArray != null) {
            DirictoryesPath = currentPath(mainDirectory, DirictoryesPathArray[countChild]);

            inputCommand.value = mainDirectory + "\>" + DirictoryesPath;
            inputCommand.focus();
        }
    }
    
    if (inputCommand.value == '')
        inputCommand.value = mainDirectory + "\>"
    
    return key
}
/**
   * JavaScript функция - имеет аннотацию с указанием типа
   */
function DownScroll() {
    inputCommand.scrollIntoView(false);
}
/**
   * JavaScript функция - имеет аннотацию с указанием типа
   */
function OffInput() {
    inputCommand.onkeypress = () => { return false };
}
/**
   * JavaScript функция - имеет аннотацию с указанием типа
   */
function OnInput() {
    inputCommand.onkeypress = () => { return true };
}
/**
   * JavaScript функция - имеет аннотацию с указанием типа
    * @param {number} status состояние выполненной команды на сервере.
     * @param {string} dataText данные пришедшие от сервера.
   */
function WriteHistoryCommand(status, dataText) {

    if (status != 2) {

        let li = document.createElement("li");
        let pre = document.createElement("pre");

        pre.append(dataText);
        li.append(pre);

        if (status == 1)
            li.classList.add("red");
        else
            li.classList.add("white");
        

        historyCommand.appendChild(li);
        DownScroll();
    }
}

/**
   * JavaScript функция - имеет аннотацию с указанием типа
   */
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
    let lineReturn = '';

    let array = pathArrayChild[0];
    for (let i = 0; i < array.length; i++) {
        if (array[i] != pathMain[i])
            lineReturn += array[i];
        
    }

    if (countChild == DirictoryesPathArray.length - 1)
        countChild = 0;
    else
        countChild++;

    lineReturn = lineReturn.slice(0);
    return lineReturn;
}
