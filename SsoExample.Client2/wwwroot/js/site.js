// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.userManager = window.userManager == undefined ? new Oidc.UserManager({
    authority: "https://localhost:44300",
    client_id: "client2",
    response_type: "code id_token token",
    scope: "openid profile api offline_access",
    silent_redirect_uri: window.location.href,
    automaticSilentRenew: true,
}) : window.userManager;

userManager.signinSilentCallback();

function silentLoginRefresh() {

    userManager.signinSilent().then(function (newUser) {
        console.log(newUser);
        location.reload();
    }).catch(function (e) {
        console.log("========  " + e);
    });
}