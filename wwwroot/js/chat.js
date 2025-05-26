"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

/*
connection.on("ReceiveMessage", function (fname, message, date) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.innerHTML = `<strong>${fname}: </strong> ${message} ................ at ${date} <br>`;
});
*/

connection.on("ReceiveMessage", function (fname, message, user_data, date, cID, id1) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    if(id1 === getCookie("cookie_auth")){
        li.innerHTML = `<div style="display:flex;">
                            <div style="margin-left:auto;">
                                <strong>${fname}: </strong><span class="message-text">${message}</span><br>
                            </div>
                        </div>`;
        if (user_data) {
            var cookie_room = getCookie("cookie_room");
            li.innerHTML += `<div style="display:flex;">
                                <div style="margin-left:auto;">
                                    <a style="text-decoration: none;" asp-area="" href="user_data/${cookie_room}/${cID}/${user_data}">${user_data} </a>
                                </div>
                             </div>`;
        }
        li.innerHTML += `<div style="display:flex;">
                            <div style="margin-left:auto;">
                                <br><span style="margin-left:50px"><small><code>at ${date}</code></small></span><br><br>
                            </div>
                        </div>`;
    }
    else{
        li.innerHTML = `<strong>${fname}: </strong><span class="message-text">${message}</span><br>`;
        if (user_data) {
            var cookie_room = getCookie("cookie_room");
            li.innerHTML += `<a style="text-decoration: none;" asp-area="" href="user_data/${cookie_room}/${cID}/${user_data}">${user_data}</a>`;
        }
        li.innerHTML += `<br><span style="margin-left:50px"><small><code>at ${date}</code></small></span><br><br>`;
    }
    li.querySelector('.message-text').innerText = message;
});

connection.on("noticeMessage", function (notice) {
    var li = document.createElement("li");
    document.getElementById("noticeList").appendChild(li);
    li.innerHTML = `<strong style="color:red;">${notice}</strong>`;
});

connection.start().then(function () {
    var IdRoom = getCookie("cookie_room");
    var auth = getCookie("cookie_auth");
    connection.invoke("GetChatData", IdRoom);
    document.getElementById("sendButton").disabled = false; // Mengaktifkan tombol "sendButton"
    connection.invoke("JoinGroup", IdRoom);
    connection.invoke("JoinGroup", auth);
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var id1 = document.getElementById("id1Input").value;
    var id2 = document.getElementById("id2Input").value;
    var idr = getCookie("cookie_room")
    var fname = getCookie("cookie_fname")
    var message = document.getElementById("messageInput").value;
    //var user_data = document.getElementById("dataInput").value;
    var fileInput = document.getElementById('dataInput');
    var user_data = fileInput.files[0].name;

    var date = document.getElementById("dateInput").value;
    connection.invoke("SendMessage", id1, id2, message, user_data, date, idr, fname).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

