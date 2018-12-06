<?php
$host = "127.0.0.1";
$user = "root";
$pass = "";
$db ="test_bankdb2";

$con = mysqli_connect($host, $user, $pass, $db) or die ('Cannot Connect : ' .mysqli_error());

$account_number = mysqli_real_escape_string($con,$_GET['account_number']);
$transaction_desc = mysqli_real_escape_string($con,$_GET['transaction_desc']);
$credit = mysqli_real_escape_string($con,$_GET['credit']);
$transaction_date = mysqli_real_escape_string($con,$_GET['transaction_date']);

$sql = "select * from accounts where account_number = '".$account_number."'";
$result = mysqli_query($con,$sql) or die ('Failed Query : ' .mysqli_error($con));

while($row = mysqli_fetch_array($result,MYSQLI_ASSOC))
{
	$id = $row['id'];
	$oldbalance = $row['balance'];
	$newBalance = $oldbalance + $credit;
}

$sql2 = "update accounts set balance = '".$newBalance."' where id = '".$id."'";
mysqli_query($con,$sql2);

$sql3 = "insert into transactions(account_number,transaction_desc,credit,transaction_date,newBalance) values ('".$account_number."','".$transaction_desc."','".$credit."','".$transaction_date."','".$newBalance."')";
mysqli_query($con,$sql3);
mysqli_close($con);

?>