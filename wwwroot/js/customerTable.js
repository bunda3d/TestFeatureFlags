$(document).ready(function () {
	$('#customerTable').dataTable({
		"processing": true, //shows processing indicator on load
		"serverSide": true,
		"filter": true, //enable search box
		//list of records that are returned from the API will be named as ‘data
		"ajax": { //call api endpoint for data
			"url": "/customer",
			"type": "POST",
			"datatype": "json"
		},
		"columnDefs": [{
			"targets": [0],
			"visible": false,
			"searchable": false
		}],
		//important to use camelCasing for variables. "firstName" will work, "FirstName" won’t.
		"columns": [
			{ "data": "id", "name": "Id", "autoWidth": true },
			{ "data": "firstName", "name": "First Name", "autoWidth": true },
			{ "data": "lastName", "name": "Last Name", "autoWidth": true },
			{ "data": "contact", "name": "Contact", "autoWidth": true },
			{ "data": "email", "name": "Country", "autoWidth": true },
			{
				"data": "dateOfBirth", "name": "Date Of Birth", "type": "date",
				"render": function (data) {
					var date = new Date(data);
					var month = date.getMonth() + 1;
					var day = date.getDate();
					return date.getFullYear() + "/" + (month.length > 1 ? month : "0" + month) + "/" + ("0" + day).slice(-2);
					//return date format like 2000/01/01;
				}, "autoWidth": true
			},
			{
				"render": function (data, row) { return "<a href='#' class='btn btn-danger' onclick=DeleteCustomer('" + row.id + "'); >Delete</a>"; }
			},
		]
	});
});