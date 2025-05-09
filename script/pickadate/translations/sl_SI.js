// Slovenian

jQuery.extend( jQuery.fn.pickadate.defaults, {
    monthsFull: [ 'januar', 'februar', 'marec', 'april', 'maj', 'junij', 'julij', 'avgust', 'september', 'oktober', 'november', 'december' ],
    monthsShort: [ 'jan', 'feb', 'mar', 'apr', 'maj', 'jun', 'jul', 'avg', 'sep', 'okt', 'nov', 'dec' ],
    weekdaysFull: [ 'nedelja', 'ponedeljek', 'torek', 'sreda', 'četrtek', 'petek', 'sobota' ],
    weekdaysShort: [ 'ned', 'pon', 'tor', 'sre', 'čet', 'pet', 'sob' ],
    today: 'danes',
    clear: 'izbriši',
    close: 'zapri',
    firstDay: 1,
    format: 'd. mmmm yyyy',
    formatSubmit: 'yyyy/mm/dd'
});

try {
    jQuery.extend(jQuery.fn.pickatime.defaults, {
        clear: 'izbriši'
    });
} catch (e) {

}
