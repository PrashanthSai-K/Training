const ApiUrl = 'https://dummyjson.com/users';

function ApiCallCallback(callback) {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", ApiUrl);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (xhr.status == 200)
                callback(null, JSON.parse(xhr.responseText));
            else
                callback(new Error("API Server error"), null);
        }
    }
    xhr.send();
}

function ApiCallPromise() {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open("GET", ApiUrl);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                if (xhr.status == 200)
                    resolve(JSON.parse(xhr.responseText));
                else
                    reject(new Error("API Server error"));
            }
        }
        xhr.send();
    })
}

async function ApiCallAsync() {
    const response = await fetch(ApiUrl);
    if (!response.ok)
        console.log("Some error during API call");
    const data = await response.json();
    return data?.users;
}

async function GetUsers(calltype) {
    const userList = document.getElementById("user-list");
    userList.innerHTML = "";
    var userdata;
    if (calltype == "callback")
        ApiCallCallback((err, data) => {
            if (err)
                console.log(err);
            else
                renderData(data?.users, "Callback");
        })
    else if (calltype == "promise")
        ApiCallPromise().then((data) => {
            userdata = data?.users;
            renderData(userdata, "Promise");
        }).catch(err => console.log(err));
    else {
        userdata = await ApiCallAsync();
        renderData(userdata, "Async/Await");
    }
}

function renderData(userdata, callType) {
    const userList = document.getElementById("user-list");
    const calltype = document.querySelector("#calltype");
    calltype.innerHTML = `Api call using ${callType}`;
    userList.innerHTML = "";
    userdata?.slice(0, 10).map((user) => {
        const li = document.createElement("li");
        li.className = "asdad";
        li.innerText = user?.username;
        userList.appendChild(li);
    })
}
