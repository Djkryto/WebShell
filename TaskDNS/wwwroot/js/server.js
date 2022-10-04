import * as site from './site.js';

let dataHistory;

async function AddCommand(dataTime, inputField, historyField) {

    let urlAdd = "https://localhost:7032/Server/Add";
    let commandJS = { id: 0, data: dataTime, textCommand: inputField};

    historyField.value += inputField + "\n";
    site.SwitcStateLoader();

    if (inputField != "") {
       await fetch(urlAdd, {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(commandJS),
            headers: { 'Content-Type': 'application/json' }
       })
    }
}



async function getHistory(historyField) {

    let urlHistory = "https://localhost:7032/Server/History"
    let response = await fetch(urlHistory);
    let data = await response.json()

    dataHistory = data;

    for (let i in data) {
        historyField.value += data[i].textCommand + "\n"
    }
}


    
async function getOutputConsole(historyField) {

    setTimeout(
        async function () {
            let urlOutput = "https://localhost:7032/Server/Output"
            let response = await fetch(urlOutput);
            let data = await response.json();

            historyField.value += data.textCommand;
        }
        , 100)
    site.SwitcStateLoader();
}



export { getOutputConsole, getHistory, AddCommand, dataHistory };