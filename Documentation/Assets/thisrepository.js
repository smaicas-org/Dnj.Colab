$(function () {
    var lang = 'en';
    var currentLanguages = [];
    $('.language').map(function (i, el) { currentLanguages.push(el.value.toLowerCase()) });

    setLanguage = function (el) {
        currentLanguages.forEach((el, i, arr) => {
            $('.' + el).removeClass('d-block');
            $('.' + el).addClass('d-none');
            $('.btn-' + el).removeClass('selected');
        });
        lang = el.value.toLowerCase();
        $('.' + lang).removeClass('d-none');
        $('.' + lang).addClass('d-block');
        $('.btn-' + lang).addClass('selected');
    }
    setLanguage({ value: 'en' });
});