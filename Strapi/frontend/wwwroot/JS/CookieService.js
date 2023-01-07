window.blazorExtensions = {

    WriteCookie: function (name, value, minutes) {

        var expires;
        if (minutes) {
            var date = new Date();
            date.setTime(date.getTime() + (minutes * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    },

    GetCookie: function (cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    DeleteCookie: function (name) {

        var expires;
            var date = new Date();
            date.setTime(date.getTime() - 60);
            expires = "; expires=" + date.toGMTString();


        document.cookie = name + "=" + "" + expires + "; path=/";
    },
}