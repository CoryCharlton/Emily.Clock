(function (global) {
    // Detect iframe embedding and suppress page chrome via CSS
    if (window.self !== window.top) {
        document.documentElement.classList.add('embedded');
    }

    function showStatus(el, msg, isError) {
        el.className = isError ? 'error' : 'success';
        el.textContent = msg;
    }

    function loadConfig(section, cb) {
        var form = document.getElementById('form');
        if (form) { form.classList.add('loading'); }
        fetch('/api/configuration/' + section)
            .then(function (r) { return r.json(); })
            .then(function (data) {
                if (form) { form.classList.remove('loading'); }
                cb(data);
            })
            .catch(function () {
                if (form) { form.classList.remove('loading'); }
                var el = document.getElementById('status');
                if (el) { showStatus(el, 'Failed to load configuration.', true); }
            });
    }

    function saveConfig(section, payload, statusEl) {
        statusEl.className = '';
        statusEl.textContent = '';
        fetch('/api/configuration/' + section, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
        .then(function (r) {
            if (r.ok) {
                showStatus(statusEl, 'Saved.', false);
            } else {
                showStatus(statusEl, 'Save failed (' + r.status + ').', true);
            }
        })
        .catch(function () {
            showStatus(statusEl, 'Save failed.', true);
        });
    }

    // "07:00:00" → "07:00"
    function timeToInput(ts) {
        if (!ts) { return ''; }
        var parts = ts.split(':');
        if (parts.length >= 2) { return parts[0] + ':' + parts[1]; }
        return ts;
    }

    // "07:00" → "07:00:00"
    function inputToTime(val) {
        if (!val) { return '00:00:00'; }
        return val.length === 5 ? val + ':00' : val;
    }

    global.App = {
        showStatus: showStatus,
        loadConfig: loadConfig,
        saveConfig: saveConfig,
        timeToInput: timeToInput,
        inputToTime: inputToTime
    };
})(this);
