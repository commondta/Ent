<!DOCTYPE html>
<html lang="en" >

<head>
    <?php require_once ('auth.php');?>
    <meta charset="UTF-8">
    <title>POS Receipt </title>

    <script src="lib/jquery.js" type="text/javascript"></script>
    <script src="src/facebox.js" type="text/javascript"></script>
    <?php
    if($_GET['invoice_number']){

//        print_r($_GET['transcation_id']);exit;
//    if($_GET['invoice']){
        $fbr_invoice_no_post=$_GET['invoice_number'];
//        print_r($name);exit;
//        $invoice=$_GET['invoice'];
        include('../connect.php');
        $result = $db->prepare("SELECT * FROM sales WHERE invoice_number= :invoice_number");
        $result->bindParam(':invoice_number', $fbr_invoice_no_post);
        $result->execute();
        //$customer = array();
        for($i=0; $row = $result->fetch(); $i++){
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
            $transaction_type = $row['transaction_type'];
            $delivery_charges = $row['delivery_charges'];
            $qr_code = $row['qr_code'];
            $fbr =  $row['fbr'];
//            print_r($transaction_id);exit;

            if($pt=='cash'){
                $cash=$row['due_date'];
                $amount=$cash-$am;


            }
        }
    }else{
        $invoice='';

    }

    //    print_r($_GET['transcation_id']);exit;

    ?>
    <?php

    include('../connect.php');

    $lastinvoiceNumber = $db->prepare("SELECT *, MAX(invoice) as finalcode FROM sales_order  ORDER  BY transaction_id DESC ");
    $lastinvoiceNumber->execute();
    $lastinvoiceNumber = $lastinvoiceNumber->fetchObject();
    $invoice_number = '';
    if($lastinvoiceNumber){
        if($lastinvoiceNumber->status == 0 && $lastinvoiceNumber->createdUserId == $_SESSION['SESS_MEMBER_ID']){
            $finalcode  = $lastinvoiceNumber->finalcode ;

        }else{
            $finalcode  = $lastinvoiceNumber->finalcode +1 ;

        }
    }else{
        $finalcode  = 1;

    }
    ?>
    <script language="javascript">
        function Clickheretoprint()
        {
            var disp_setting="toolbar=yes,location=no,directories=yes,menubar=yes,";
            disp_setting+="scrollbars=yes,width=700, height=400, left=100, top=25";
            var content_vlue = document.getElementById("content").innerHTML;

            var docprint=window.open("","",disp_setting);
            docprint.document.open();
            docprint.document.write('</head><body onLoad="self.print()" style="width: 700px; font-size:11px; font-family:arial; font-weight:normal;">');
            docprint.document.write(content_vlue);
            docprint.document.close();
            docprint.focus();
        }
    </script>
</head>

<body>

<!--<button  style="float:right;" class="btn btn-success btn-mini"><a href="javascript:Clickheretoprint()"> Print</button></a>-->

<div id="invoice-POS">
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
            background: var(--lm-danger);
            color: #FFF;
        }
        #invoice-POS ::moz-selection {
            background: var(--lm-danger);
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
            font-size: 11px;
            color: black;
            line-height: 10px;
        }
        #invoice-POS #top, #invoice-POS #mid, #invoice-POS #bot {
            /* Targets all id with 'col-' */
            border-bottom: 1px solid #EEE;
        }
        #invoice-POS #top {
            min-height: 70px;
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
            background: var(--lm-border);
        }
        #invoice-POS .service {
            border-bottom: 1px solid var(--lm-border);
        }
        #invoice-POS .item {
            width: 30mm;
        }
        #invoice-POS .itemtext {
            font-size: 11px;
            line-height: 0  ;
        }
        #invoice-POS #legalcopy {
            margin-top: 5mm;
        }
        @media print {
            #invoice-POS .service {
                border-bottom: 1px solid var(--lm-border);
            }
        }
    </style>
    <center id="top">
        <div >
            <img  src="img/logo.png"  style="width: 70%;   margin: 24px 0;" alt="English">
            <p style="font-family: monotype corsiva;font-size: 18px;font-weight: bold;    margin-top: -20px;">
                " Experience The Best Buying "
            </p>
        </div>
        <!--        <div class="info">-->
        <!--            <h2 style="margin: 0">Baba Bakers & Sweets</h2>-->
        <!--        </div><!--End Info-->
    </center><!--End InvoiceTop-->

    <div id="mid">
        <div class="info" style="font: menu;font-weight: 600">
            <style>
                .pera{
                    text-align: center;
                    line-height: 1;
                }

            </style>
            <p class="pera" style="line-height: 10px;color: black!important;margin-top: 6px;line-height: 1">
                Office No.6,7,8, 1st floor, Rehman Electronics <br> Center, 22 Yasin street, Hall Road Lahore
            </p>
            <p class="pera" style="line-height: 4px;color: black!important;">04237233586-87 - 0323/03015755775</p>
            <p class="pera" style="line-height: 4px;color: black!important;">https://epro.pk </p>
            <!--            <p class="pera" style="line-height: 4px;color: black!important;">	Email :info@baba.pk   </p>-->
            <p class="pera" style="line-height: 4px;color: black!important;">	NTN : A028320-6 </p>
            <!--            <p class="pera" style="line-height: 4px;color: black!important;">GST NO: 3277876193445 </p>-->
        </div>
    </div>
    <!--End Invoice Mid-->
    <div id="">
        <?php
        if($fbr == 1){
            $tax = 18;
        }else{
            $tax = 0;

        }
        $resulta = $db->prepare("SELECT * FROM customer WHERE customer_name= :a");
        $resulta->bindParam(':a', $cname);
        $resulta->execute();
        for($i=0; $rowa = $resulta->fetch(); $i++){
            $address=$rowa['address'];
            $contact=$rowa['contact'];
        }
        ?>
        <div class="" style="width: 100%">
            <div style="width: 50%;float: left;font: small-caption">
                <p style="font-size: 13px;color: black !important;">Transaction : <?php echo $invoice ?></p>
                <p style="font-size: 13px;color: black !important;width: 200px">Date : <?php echo $date .' '. $time ?></p>
                <p style="font-size: 13px;color: black !important;width: 275px">Customer Name : <?php echo $customer; ?></p>



                <!--						<p style="font-size: 13px;color: black !important;">Time : --><?php //echo $time; ?><!--</p>-->


            </div>
            <div style="width: 50%;float: right;font: small-caption">

                <!--						<p style="font-size: 13px;color: black!important;" >TAX Office. : CTO Lahore</p>-->
                <p style="font-size: 13px;color: black!important;" >POS Reg. No. : 146669</p>
                <!--                <p style="font-size: 13px;color: black !important;margin-left: 23px">Terminal : T011001</p>-->
                <?php  if(isset($_GET['tp'])){
                $pt = $_GET['tp'];
                }
                ?>
                <p style="font-size: 13px;color: black !important;margin-left:30px">Mop : <?php
                    //                    print_r($pt);exit;
                    if($transaction_type == 'BIL'){
                    if($pt == 'credit_customer'){
                    echo 'Credit Sales';
                    }elseif($pt == 'credit'){
                    echo 'Credit Card';
                    }elseif($pt == 'return'){
                    echo 'Sales Return';
                    }
                    elseif($pt == 'credit_sale'){
                    echo 'Credit Sale';
                    } elseif($pt == 'both'){
                    echo 'Card + Cash';
                    }elseif($pt == 'cod'){
                    echo 'COD';
                    }else{
                    echo 'Cash Sales';

                    }
                    }else{
                    echo 'Return Sales';
                    } ?>


                </p>

                <!--						<p style="font-size: 13px;color: black !important;">Sales Point : Johar Town, Lahore</p>-->
            </div>

        </div>

    </div>
    <div id="bot">

        <div id="table">
            <table style="font: menu;font-weight: 600">
                <tr class="tabletitle">

                    <td class="item"> Product </td>
                    <td class="item"> Price  </td>
                    <?php if($fbr == 1){ ?>

                    <td class="item"> GST Rate </td>
                    <td class="item"> GST val </td>
                    <?php } ?>
                    <td class="item"> Qty </td>

                    <td class="item"> Total </td>

                </tr>
                <?php
                $id=$invoice;

                $result = $db->prepare("SELECT * FROM sales_order WHERE invoice= :userid AND isDeleted=0 AND status = 1");
                $result->bindParam(':userid', $id);
                $result->execute();

                $gstcount = array();
                $valInctax = array();
                $totalDiscount = array();
                $count = 1;
                for($i=0; $row = $result->fetch(); $i++){

                $totalDiscount[$i]= $row['discount'];
                ?>

                <tr class="service">

                    <td class="tableitem"  style="min-width: 100px !Important"><p class="itemtext" style=";color: black!important;line-height: 1"><?php echo $row['name']; ?></p></td>

                    <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                            <?php
                            echo $row['price'] ;
                            ?></p>
                    </td>
                    <?php if($fbr == 1){ ?>
                    <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                            <?php

                            echo '18%';



                            ?>

                        </p></td>

                    <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                            <?php

                            echo  $gstcount[] = ($row['price'] * $row['qty'] / 100) * $tax;
                            //											if (!empty($row['discount'])) {
                            //												echo $amount[$i] = (($row['price'] * $row['qty']) + $tax_amount) - $row['discount'];
                            //											} else {
                            //												echo $amount[$i] = (($row['price'] * $row['qty']) + $tax_amount);
                            //											};



                            //												$percentage = 0.18;
                            //												$per = 1.18;
                            //
                            //											echo $gstcount[]= ($row['discount']) ?  round(((($row['price'] - ($row['discount'] / $row['qty']))) /$per) * $percentage ) :  round(((($row['price'])) /$per) * $percentage )  ;

                            ?>

                        </p></td>
                    <?php } ?>
                    <td class="tableitem"><p class="itemtext" style=";color: black!important;"><?php echo $row['qty']; ?></p></td>

                    <td class="tableitem"><p class="itemtext" style=";color: black!important;">
                            <?php

                            $tax_amount = ($row['price'] * $row['qty'] / 100) * $tax;
                            if (!empty($row['discount'])) {
                            echo $valInctax[$i] = (($row['price'] * $row['qty']) + $tax_amount) - $row['discount'];
                            } else {
                            echo $valInctax[$i] = (($row['price'] * $row['qty']) + $tax_amount);
                            };


                            //												$percentage1 = 0.18;
                            //												$pe2r = 1.18;
                            //
                            //											echo  $total =  ($row['discount']) ? ((($row['amount'] - $row['discount']) / $pe2r)+ ((($row['amount'] - $row['discount'])) /$pe2r) * $percentage) : ((($row['amount']) / $pe2r)+ ((($row['amount'])) /$pe2r) * $percentage) ;
                            //											$valInctax[$i] = $total;

                            ?>
                        </p></td>
                </tr>

                <?php   $count++;
                }
                ?>


                <?php if($fbr == 1){ ?>
                <tr class="tabletitle" style="background-color: white;line-height: 0">
                    <td class="Rate" colspan="3"><h2>GST Total:</h2></td>
                    <td class="payment"><h2><?php echo  array_sum($gstcount); ?></h2></td>

                </tr>
                <?php } ?>
                <tr class="tabletitle" style="background-color: white;line-height: 0">

                    <td class="Rate"  colspan="3"><h2>Total:</h2></td>

                    <td class="payment"><h2>  <?php

                            echo array_sum($valInctax);
                            $sdsd=$invoice;
                            $resultas = $db->prepare("SELECT sum(amount) FROM sales_order WHERE invoice= :a");
                            $resultas->bindParam(':a', $sdsd);
                            $resultas->execute();
                            for($i=0; $rowas = $resultas->fetch(); $i++){
                            $fgfg=$rowas['sum(amount)'];
                            }
                            ?></h2></td>
                </tr>

                <?php if($fbr == 1){ ?>

                <tr class="tabletitle" style="background-color: white;line-height: 0">

                    <td class="Rate" colspan="3"><h2>POS Service Fee:</h2></td>

                    <td class="payment"><h2>1</h2></td>
                </tr>
                <?php } ?>
                <?php if($delivery_charges){ ?>

                <tr class="tabletitle" style="background-color: white;line-height: 0">

                    <td class="Rate" colspan="3"><h2>Delivery Charges:</h2></td>

                    <td class="payment"><h2><?php echo $delivery_charges; ?></h2></td>
                </tr>
                <?php } ?>
                <tr class="tabletitle" style="background-color: white;line-height: 0">

                    <td class="Rate" colspan="3"><h2>Net Total: </h2></td>

                    <td class="payment"><h2><?php

                            if($fbr == 1){
                            echo($delivery_charges)? array_sum($valInctax) + 1 + $delivery_charges : array_sum($valInctax) + 1;

                            }else{
                            echo($delivery_charges)? array_sum($valInctax)  + $delivery_charges : array_sum($valInctax);

                            }




                            //										echo	(array_sum($valInctax) + 1) ;
                            ?></h2></td>
                </tr>
                <?php if($pt == 'cash'){ ?>

                <tr class="tabletitle" style="background-color: white;line-height: 0">

                    <td class="Rate" colspan="3"><h2>Cash:</h2></td>

                    <td class="payment"><h2><?php echo $paidCash; ?></h2></td>
                </tr>
                <?php if($pt=='cash'){
                ?>
                        <!--				<tr>-->
                <!--					<td colspan="3"style=" text-align:right;"><strong style="font-size: 12px; color: #222222;">Change Back (cash):&nbsp;</strong></td>-->
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

                    <td class="Rate" colspan="3"><h2> <?php
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
                            echo round($cash);
                            }
                            if($pt=='cash'){

                            if($fbr == 1){
                            echo round($paidCash - ((array_sum($valInctax))   + 1));

                            }else{
                            echo round($paidCash - ((array_sum($valInctax))   ));

                            }




                            //						print_r($amount);exit;
                            //						echo formatMoney($amount, true);



                            }
                            ?></h2></td>
                </tr>
                <?php } ?>

            </table>
        </div><!--End Table-->
        ===============
        <p style="margin: 0;font: menu">Sales Person : <?php echo $cashier ?></p>
        ===============
        <?php if($fbr == 1){ ?>

        <p style="font-size: 14px;color: black!important;font-weight: 600	" >FBR Invoice No. : <?php echo $fbr_invoice_no ?></p>

        <?php }?>


        <div id="legalcopy" style="margin-top: 0;font: small-caption">
            <p style="line-height: 1.2;color: black !important;margin-top: 0;margin-bottom: 0">
                Thank you for your visit.
                <br>
                <?php if($fbr == 1){ ?>
                All Prices are inclusive of sales tax,wherever is applicable.<br>
                <?php }else{ ?>
                All Prices are Exclusive of sales tax.<br>

                <?php } ?>
                Exchange will not be accepted without invoice.<br>
                To Order Online, visit : https://epro.pk
            </p>
        </div>
        <?php if($fbr == 1){ ?>

        <div id="legalcopy"  style="margin-top: 0;">


            <div class="" style="width: 100%">
                <div style="width: 50%;float: left">
                    <img style='width: 95px;height: 82px;margin-top: 1px' src='images/logofbr.PNG'>

                </div>
                <div style="width: 40%;float: right;padding-right: 25px">

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
            <div id="legalcopy" style="margin-top: 0;">
                <p style="line-height: 1.2;font-size: 13px;color: black !important;margin-top: 100px;width: 255px;font:menu">
                    Verify this Invoice through FBR TaxAsaan MobileApp or Sms at 9966 and win exciting prizes in draw
                    <br>
                </p>



            </div>
        <?php }else{?>
            <div style="margin-bottom: 20px"></div>
        <?php }?>
    </div><!--End InvoiceBot-->
</div><!--End Invoice-->



</body>

</html>