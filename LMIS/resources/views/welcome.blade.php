<!DOCTYPE html>
<html>
<head>
    <!--	--><?php //require_once ('auth.php');?>
            <!--	<title>-->
    <!--		POS-->
    <!--	</title>-->
    <!--	<link href="css/bootstrap.css" rel="stylesheet">-->

    <link rel="stylesheet" type="text/css" href="css/DT_bootstrap.css">

    <link rel="stylesheet" href="css/font-awesome.min.css">
    <style type="text/css">

        .sidebar-nav {
            padding: 9px 0;
        }
    </style>
    <style>
        @media print {
            #noprint {
                visibility: hidden;
            }
        }
    </style>
    <!--	<link href="css/bootstrap-responsive.css" rel="stylesheet">-->
    <link href="../style.css" media="screen" rel="stylesheet" type="text/css" />
    <link href="src/facebox.css" media="screen" rel="stylesheet" type="text/css" />
    <script src="lib/jquery.js" type="text/javascript"></script>
    <script src="src/facebox.js" type="text/javascript"></script>

    <script language="javascript">

        function Clickheretoprint()
        {
            var disp_setting="toolbar=yes,location=no,directories=yes,menubar=yes,";
//			disp_setting+="scrollbars=yes, left=0, top=0,right=0";
            disp_setting+="scrollbars=yes,width=76mm";
            var content_vlue = document.getElementById("invoice-POS").innerHTML;

            var docprint=window.open("","",disp_setting);
            docprint.document.open();
//			docprint.document.write('</head><body onLoad="self.print()" >');
            docprint.document.write('</head><body onLoad="self.print()" style="width: 76mm; font-size: 13px;">');
            docprint.document.write(content_vlue);
            docprint.document.close();
            docprint.focus();
        }
    </script>
    <?php
    $invoice=$_GET['invoice'];
    include('../connect.php');
    $result = $db->prepare("SELECT * FROM sales WHERE invoice_number= :userid");
    $result->bindParam(':userid', $invoice);
    $result->execute();
    //$customer = array();
    for($i=0; $row = $result->fetch(); $i++){
//		echo '<pre>'; print_r($row);exit;
        $cname=$row['name'];
        $customer = $row['name'];
        $invoice=$row['invoice_number'];
        $date=$row['date'];
        $time=$row['time'];
        $fbr_invoice_no=$row['fbr_invoice'];
        $cash=$row['due_date'];
        $cashier=$row['cashier'];
        $paidCash=$row['due_date'];
        $pt=$row['type'];
        $am=$row['amount'];
        $transaction_id = $row['transaction_id'];

        if($pt=='cash'){
            $cash=$row['due_date'];
            $amount=$cash-$am;


        }
    }
    ?>
    <?php
    function createRandomPassword() {
        $chars = "003232303232023232023456789";
        srand((double)microtime()*1000000);
        $i = 0;
        $pass = '' ;
        while ($i <= 7) {

            $num = rand() % 33;

            $tmp = substr($chars, $num, 1);

            $pass = $pass . $tmp;

            $i++;

        }
        return $pass;
    }
    $finalcode='RS-'.createRandomPassword();
    ?>



    <script language="javascript" type="text/javascript">
        /* Visit http://www.yaldex.com/ for full source code
         and get more free JavaScript, CSS and DHTML scripts! */
        <!-- Begin
        var timerID = null;
        var timerRunning = false;
        function stopclock (){
            if(timerRunning)
                clearTimeout(timerID);
            timerRunning = false;
        }
        function showtime () {
            var now = new Date();
            var hours = now.getHours();
            var minutes = now.getMinutes();
            var seconds = now.getSeconds()
            var timeValue = "" + ((hours >12) ? hours -12 :hours)
            if (timeValue == "0") timeValue = 12;
            timeValue += ((minutes < 10) ? ":0" : ":") + minutes
            timeValue += ((seconds < 10) ? ":0" : ":") + seconds
            timeValue += (hours >= 12) ? " P.M." : " A.M."
            document.clock.face.value = timeValue;
            timerID = setTimeout("showtime()",1000);
            timerRunning = true;
        }
        function startclock() {
            stopclock();
            showtime();
        }
        window.onload=startclock;
        // End -->
    </SCRIPT>
    <style>
        /*Downloaded from https://www.codeseek.co/Sambra22/pos-receipt-template-html-css-JNexJP */
        #invoice-POS {
            box-shadow: 0 0 1in -0.25in rgba(0, 0, 0, 0.5);
            padding: 2mm;
            margin: 0 auto;
            width: 76mm;
            background: #FFF;
        }
        #invoice-POS ::selection {
            background: #f31544;
            color: #FFF;
        }
        #invoice-POS ::moz-selection {
            background: #f31544;
            color: #FFF;
        }
        #invoice-POS h1 {
            font-size: 1.5em;
            color: #222;
        }
        #invoice-POS h2 {
            font-size: 15px;
        }
        #invoice-POS h3 {
            font-size: 1.2em;
            font-weight: 300;
            line-height: 2em;
        }
        #invoice-POS p {
            font-size: 13px;
            color: black;
            line-height: 10px;
        }
        #invoice-POS #top, #invoice-POS #mid, #invoice-POS #bot {
            /* Targets all id with 'col-' */
            border-bottom: 1px solid #EEE;
        }
        #invoice-POS #top {
            min-height: 100px;
        }
        #invoice-POS #mid {
            min-height: 80px;
        }
        #invoice-POS #bot {
            min-height: 50px;
        }
        #invoice-POS #top .logo {
            height: 60px;
            width: 60px;
            background: none;
            background-size: 60px 60px;
        }
        #invoice-POS .clientlogo {
            float: left;
            height: 60px;
            width: 60px;
            background: none;
            background-size: 60px 60px;
            border-radius: 50px;
        }
        #invoice-POS .info {
            display: block;
            margin-left: 0;
        }
        #invoice-POS .title {
            float: right;
        }
        #invoice-POS .title p {
            text-align: right;
        }
        #invoice-POS table {
            width: 100%;
            border-collapse: collapse;
        }
        #invoice-POS .tabletitle {
            font-size: 11px;
            background: #EEE;
        }
        #invoice-POS .service {
            border-bottom: 1px solid #EEE;
        }
        #invoice-POS .item {
            width: 24mm;
        }
        #invoice-POS .itemtext {
            font-size: 11px;
            line-height: 1.3;
        }
        #invoice-POS #legalcopy {
            margin-top: 5mm;
        }

    </style>
<body>

<?php //include('navfixed.php');?>
<div class="container-fluid">
    <div class="row-fluid">


        <div class="span10" >
            <a id="noprint" href="sales.php?id=cash&invoice=<?php echo $finalcode ?>"><button class="btn btn-default"><i class="icon-arrow-left"></i> Back to Sales</button></a>


            <div id="invoice-POS">

                <center id="top">
                    <div >
                        <img style="width: 100px;margin-bottom: 10px" src="img/logo.png"  alt="English">
                    </div>
                    <div class="info">
                        <h2>ACHHA MILK SHOP</h2>
                    </div><!--End Info-->
                </center><!--End InvoiceTop-->

                <div id="mid">
                    <div class="info">
                        <style>
                            .pera{
                                text-align: center;
                                line-height: 1;
                            }

                        </style>
                        <p class="pera" style="line-height: 10px;color: black!important;">Outside Bhati Gate, Chowk , Data Darbar, Lahore Data Gunj Bukhsh Town  </p>
                        <p class="pera" style="line-height: 4px;color: black!important;">Tel  +92 42 371 638 94</p>
                        <p class="pera" style="line-height: 4px;color: black!important;">www.achhafoods.com </p>
                        <p class="pera" style="line-height: 4px;color: black!important;">	Email : foods.achha@gmail.com </p>
                        <p class="pera" style="line-height: 4px;color: black!important;">	NTN : 7203849-6 </p>

                    </div>
                </div><!--End Invoice Mid-->
                <div id="legalcopy">
                    <?php
                    $address='OUTSIDE BHATI GATE, CHOWK, DATA DARBAR, Lahore Data Gunj Bukhsh Town';
                    $contact='923008590766';

                    ?>

                    <div class="" style="width: 100%">
                        <div style="width: 50%;float: left">
                            <p style="font-size: 11px;color: black !important;">Transaction : <?php echo $invoice ?></p>
                            <p style="font-size: 11px;color: black !important;">Terminal : T011001</p>
                            <p style="font-size: 11px;color: black !important;">Date : <?php echo $date ?></p>
                            <p style="font-size: 11px;color: black !important;">Time : <?php echo $time; ?></p>
                            <p style="font-size: 11px;color: black !important;">Sales Point : Green Forts 2, Lahore</p>
                            <p style="font-size: 11px;color: black !important;">Customer Name : <?php echo 'Walkin Customer'; ?></p>


                        </div>
                        <div style="width: 50%;float: right">
                            <p style="font-size: 11px;color: black!important;" >FBR Invoice No. : <?php echo $fbr_invoice_no ?></p>
                            <p style="font-size: 11px;color: black!important;" >TAX Office. : LTO Lahore</p>
                            <p style="font-size: 11px;color: black!important;" >POS Reg. No. : 162965</P>
                        </div>
                    </div>
                </div>
                <div id="bot">

                    <div id="table">
                        <table>
                            <tr class="tabletitle">
                                <td class="item">  Name </td>
                                <td class="item"> Qty </td>
                                <td class="item"> Rate </td>
                                <td class="item"> Exl. Sale Val </td>
                                <td class="item"> GST(%) </td>
                                <td class="item"> GST val </td>

                                <td class="item"> Val Inc. GST </td>
                                <!--                    <td class="item"><h2>Item</h2></td>-->
                                <!--                    <td class="Hours"><h2>Qty</h2></td>-->
                                <!--                    <td class="Rate"><h2>Sub Total</h2></td>-->
                            </tr>
                            <?php
                            $id=$invoice;

                            $result = $db->prepare("SELECT * FROM sales_order WHERE invoice= :userid");
                            $result->bindParam(':userid', $id);
                            $result->execute();

                            $gstcount = array();
                            $valInctax = array();
                            $totalDiscount = array();
                            for($i=0; $row = $result->fetch(); $i++){

                            $totalDiscount[$i]= $row['discount'];
                            ?>

                            <tr class="service">
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;"><?php echo $row['name']; ?></p></td>
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;"><?php echo $row['qty']; ?></p></td>
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                                        <?php $ppp=$row['price'];
                                        echo $ppp;
                                        //                                        echo formatMoney($ppp, true);
                                        ?></p>
                                </td>
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                                        <?php
                                        $ddd=$row['price'] * $row['qty'];
                                        echo $ddd;
                                        //                                        echo formatMoney($ddd, true);
                                        ?>
                                    </p></td>
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;">0 %</p></td>
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                                        <?php
                                        $percentage = 0;
                                        $totalWidth = $row['amount'];

                                        echo	$gstcount[$i] = ($percentage / 100) * $totalWidth; ?>

                                    </p></td>
                                <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                                        <?php
                                        $percentage = 0;
                                        $totalWidth = $row['amount'];

                                        $new_width = ($percentage / 100) * $totalWidth;
                                        echo $valInctax[$i] = $new_width + $row['amount'];
                                        ?>
                                    </p></td>
                            </tr>

                            <?php
                            }
                            ?>


                            <tr class="tabletitle" style="background-color: white;line-height: 0">
                                <td class="Rate" colspan="5"><h2>GST Total:</h2></td>
                                <td class="payment"><h2><?php echo array_sum($gstcount); ?></h2></td>

                            </tr>

                            <tr class="tabletitle" style="background-color: white;line-height: 0">

                                <td class="Rate"  colspan="5"><h2>Total:</h2></td>

                                <td class="payment"><h2>  <?php

                                        echo array_sum($valInctax);
                                        $sdsd=$invoice;
                                        $resultas = $db->prepare("SELECT sum(amount) FROM sales_order WHERE invoice= :a");
                                        $resultas->bindParam(':a', $sdsd);
                                        $resultas->execute();
                                        for($i=0; $rowas = $resultas->fetch(); $i++){
                                        $fgfg=$rowas['sum(amount)'];
                                        //					echo formatMoney($fgfg, true);
                                        }
                                        ?></h2></td>
                            </tr>
                            <tr class="tabletitle" style="background-color: white;line-height: 0">

                                <td class="Rate" colspan="5"><h2>Available Discount:</h2></td>

                                <td class="payment"><h2>  <?php
                                        echo array_sum($totalDiscount);
                                        ?></h2></td>
                            </tr>
                            <tr class="tabletitle" style="background-color: white;line-height: 0">

                                <td class="Rate" colspan="5"><h2>POS Service Fee:</h2></td>

                                <td class="payment"><h2>1</h2></td>
                            </tr>
                            <tr class="tabletitle" style="background-color: white;line-height: 0">

                                <td class="Rate" colspan="5"><h2>Net Total: </h2></td>

                                <td class="payment"><h2><?php
                                        echo	(array_sum($valInctax) - array_sum($totalDiscount))   + 1;
                                        ?></h2></td>
                            </tr><tr class="tabletitle" style="background-color: white;line-height: 0">

                                <td class="Rate" colspan="5"><h2>Cash:</h2></td>

                                <td class="payment"><h2><?php echo $paidCash; ?></h2></td>
                            </tr>
                            <?php if($pt=='cash'){
                            ?>
                                    <!--				<tr>-->
                            <!--					<td colspan="5"style=" text-align:right;"><strong style="font-size: 12px; color: #222222;">Change Back (cash):&nbsp;</strong></td>-->
                            <!--					<td style="text-align: center"  colspan="2"><strong style="font-size: 12px; color: #222222;">-->
                            <!--					--><?php
                            //					echo formatMoney($cash, true);
                            //					?>
                                    <!--					</strong></td>-->
                            <!--				</tr>-->
                            <?php
                            }
                            ?>

                            <tr class="tabletitle" style="background-color: white;line-height: 0">

                                <td class="Rate" colspan="5"><h2> <?php
                                        if($pt=='cash'){
                                        echo 'Change Back (Cash):';
                                        }
                                        if($pt=='credit'){
                                        echo 'Due Date:';
                                        }
                                        ?></h2></td>

                                <td class="payment"><h2>    <?php
                                        function formatMoney($number, $fractional=false) {
                                        if ($fractional) {
                                        $number = sprintf('%.2f', $number);
                                        }
                                        while (true) {
                                        $replaced = preg_replace('/(-?\d+)(\d\d\d)/', '$1,$2', $number);
                                        if ($replaced != $number) {
                                        $number = $replaced;
                                        } else {
                                        break;
                                        }
                                        }
                                        return $number;
                                        }
                                        if($pt=='credit'){
                                        echo $cash;
                                        }
                                        if($pt=='cash'){
                                        echo	$paidCash - ((array_sum($valInctax) - array_sum($totalDiscount))   + 1);
                                        //						print_r($amount);exit;
                                        //						echo formatMoney($amount, true);



                                        }
                                        ?></h2></td>
                            </tr>

                        </table>
                    </div><!--End Table-->
                    ===============
                    <div id="legalcopy" style="margin-top: 0">
                        <p style="line-height: 1.2;color: black !important;">
                            Thank you for your visit.
                            <br>
                            Exchange will not be accepted against perishable products.<br>
                            For Delivery, Call Now : 03 111 333 337<br>
                            To Order Online, visit : www.achhaemart.com
                        </p>



                    </div>

                    <div id="legalcopy">


                        <div class="" style="width: 100%">
                            <div style="width: 50%;float: left">
                                <img style='width: 95px;height: 82px;margin-top: 13px' src='images/logofbr.png'>
                            </div>
                            <div style="width: 50%;float: right">
                                <!--                    <img style='width: 95px' src='images/logofbr.png'>-->

<?php


include('../phpqrcode/qrlib.php');
//				include('phpqrcode/qrlib.php');
$text=$fbr_invoice_no;
$folder="images/";
$file_name= $transaction_id.".png";

//				$file_name="qr.png";
$file_name=$folder.$file_name;
QRcode::png($text,$file_name);
//							print_r($qr_code);
//							                            echo"<img style='float: right;' src='$qr_code'>";
//							echo"<img style='float: right;margin-top: 20px;width: 185px;' src=".'images/'.$transaction_id.'.png'>";

$sql = "UPDATE sales
							       		SET qr_code=?
										WHERE transaction_id=?";
$q = $db->prepare($sql);
$q->execute(array($file_name,$transaction_id));



//To Display Code Without Storing
//				QRcode::png($text);

?>
                                        <img style='float: right;' src='images/<?php echo $transaction_id ?>.png'>


                                    </div>

                                </div>

                            </div>
                            <div id="legalcopy">
                                <p style="line-height: 1.2;font-size: 11px;color: black !important;margin-top:130px">
                                    Verify this Invoice through FBR TaxAsaan MobileApp or Sms at 9966 and win exciting prizes in draw
                                    <br>
                                </p>



                            </div>

                        </div><!--End InvoiceBot-->
                    </div>
                    <!--		<div class="pull-right" style="margin-right:100px;">-->
                    <!--			<a href="javascript:Clickheretoprint()" style="font-size:20px;"><button class="btn btn-success btn-large"><i class="icon-print"></i> Print</button></a>-->
                    <!--		</div>-->
                </div>
            </div>


