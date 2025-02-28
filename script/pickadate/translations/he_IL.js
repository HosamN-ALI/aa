// Hebrew

jQuery.extend( jQuery.fn.pickadate.defaults, {
    monthsFull: [ 'ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי', 'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר' ],
    monthsShort: [ 'ינו', 'פבר', 'מרץ', 'אפר', 'מאי', 'יונ', 'יול', 'אוג', 'ספט', 'אוק', 'נוב', 'דצמ' ],
    weekdaysFull: [ 'יום ראשון', 'יום שני', 'יום שלישי', 'יום רביעי', 'יום חמישי', 'יום ששי', 'יום שבת' ],
    weekdaysShort: [ 'א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ש' ],
    today: 'היום',
 close: 'סגור',    clear: 'למחוק',
    format: 'yyyy mmmmב d dddd',
    formatSubmit: 'yyyy/mm/dd'
});

try {
    jQuery.extend(jQuery.fn.pickatime.defaults, {
        clear: 'למחוק'
    });

} catch (e) {

}