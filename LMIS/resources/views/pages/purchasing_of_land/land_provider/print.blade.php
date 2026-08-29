<link href="{{ asset('public/vendors/bootstrap4/bootstrap.min.css') }}" rel="stylesheet" id="bootstrap-css">
<script src="{{ asset('public/vendors/bootstrap4/bootstrap.min.js') }}"></script>
<script src="{{ asset('public/vendors/jquery/jquery-3.6.0.min.js') }}"></script>

<style>
    #invoice {
        padding: 30px;
    }

    .invoice {
        position: relative;
        background-color: #FFF;
        min-height: 680px;
        padding: 15px
    }

    .invoice header {
        padding: 10px 0;
        /*margin-bottom: 20px;*/
        /*border-bottom: 1px solid var(--lm-ink)*/
    }

    .invoice .company-details {
        text-align: right
    }

    .invoice .company-details .name {
        margin-top: 0;
        margin-bottom: 0
    }

    .invoice .contacts {
        margin-bottom: 20px
    }

    .invoice .invoice-to {
        text-align: left;
        margin-top: 15px;
    }

    .invoice .invoice-to .to {
        margin-top: 0;
        margin-bottom: 0
    }

    .invoice .invoice-details {
        /*text-align: right;*/
        padding-left: 20%;
        margin-top: 15px;
    }

    .invoice .invoice-details .invoice-id {
        margin-top: 0;
        color: var(--lm-ink)
    }

    .invoice main {
        padding-bottom: 50px
    }

    .invoice main .thanks {
        margin-top: -100px;
        font-size: 2em;
        margin-bottom: 50px
    }

    .invoice main .notices {
        padding-left: 6px;
    }

    .invoice main .notices .notice {
        font-size: 14px;
        font-weight: 500;
    }

    .invoice table {
        width: 100%;
        border-collapse: collapse;
        border-spacing: 0;
        margin-bottom: 20px
    }

    th {
        text-align: inherit;
        background: var(--lm-ink);
        height: 40px;
        color: white;
        border-bottom: 1px solid var(--lm-border);
        border-top: 1px solid var(--lm-border);
    }

    /*.invoice table td,.invoice table  {*/
    /*padding: 15px;*/
    /*background: var(--lm-ink);*/
    /*color: white;*/
    /*border-bottom: 1px solid var(--lm-border);*/
    /*}*/

    .invoice table th {
        white-space: nowrap;
        font-weight: 500;
        font-size: 14px
    }

    .invoice table td h3 {
        margin: 0;
        font-weight: 400;
        color: var(--lm-ink);
        font-size: 1.2em
    }

    .invoice table .qty, .invoice table .total, .invoice table .unit {
        text-align: right;
        font-size: 1.2em
    }

    .invoice table .no {
        color: #fff;
        font-size: 1.6em;
        background: var(--lm-ink)
    }

    .invoice table .unit {
        background: var(--lm-border)
    }

    .invoice table .total {
        background: var(--lm-ink);
        color: #fff
    }

    .invoice table tbody tr:last-child td {
        border: none
    }

    .invoice table tfoot td {
        background: 0 0;
        border-bottom: none;
        white-space: nowrap;
        text-align: right;
        padding: 10px 20px;
        font-size: 1.2em;
        border-top: 1px solid #aaa
    }

    .invoice table tfoot tr:first-child td {
        border-top: none
    }

    .invoice table tfoot tr:last-child td {
        color: black;
        font-size: 14px;
        font-weight: 700;
        border: 1px solid var(--lm-border);
    }

    .invoice table tfoot tr td:first-child {
        border: 1px solid var(--lm-border);
    }

    /*.invoice footer {*/
    /*width: 100%;*/
    /*text-align: center;*/
    /*color: #777;*/
    /*border-top: 1px solid #aaa;*/
    /*padding: 8px 0*/
    /*}*/

    .master-span {
        width: 140px;
        float: left;
        font-size: 13px;
        font-weight: 700;
        color: black !important;
    }

    @media print {
        .invoice {
            font-size: 11px !important;
            overflow: hidden !important
        }

        /*.invoice footer {*/
        /*position: absolute;*/
        /*bottom: 10px;*/
        /*page-break-after: always*/
        /*}*/
        .invoice > div:last-child {
            page-break-before: always
        }

        #myButton {
            display: none;
        }
    }

    .rowtd {
        border: 1px solid var(--lm-border) !important;
        text-align: center;
    }
    p{
        font-weight: 500;
        font-size: 13px;
        padding-top: 2px;
    }
    td{
        font-size: 13px;
        font-weight: 500;
    }
    tr{
        border-width: 1px;
    }
    tfoot{
        border-width: 1px;

    }
    th{
        border: 1px solid var(--lm-border);
    }
</style>
<!--Author      : @arboshiki-->

<div class="page-content">
    <div class="page-title-box">
        <div class="container-fluid">
            <div class="row align-items-center">
                <div class="col-sm-6">
                    <div class="page-title">
                        <h4>Sales Invoice</h4>
                        <ol class="breadcrumb m-0">
                            <li class="breadcrumb-item"><a href="javascript: void(0);">Admin</a></li>
                            <li class="breadcrumb-item active"><a
                                    href="<?php echo base_url('admin/sales_order/all'); ?>">Sales Invoice</a></li>
                        </ol>
                    </div>
                </div>
                <div class="col-sm-6">
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="page-content-wrapper">
            <div id="invoice">

                <div class="toolbar hidden-print">
                    <div class="text-right">

                    </div>
                    <hr>
                </div>
                <div class="invoice overflow-auto">
                    <div style="min-width: 600px">
                        <header>
                            <div class="row" style="margin: 0">
                                <div class="col">
                                    <a target="_blank" href="<?php echo base_url(); ?>">

                                        <img src="<?php echo base_url(); ?>Content/logo/logo.png" alt="" height="60"
                                             data-holder-rendered="true">

                                    </a>
                                </div>
                                <div class="col company-details" style="text-align: left;padding-left: 35%;">

                                    <div>Plot #199,Sunder Industrial Estate,Raiwind Road,Lahore</div>
                                    <div>Phone : 042-35297716-8</div>
                                    <div>NTN # : 4151066-6</div>
                                    <div>STRN # : 03-00-4151-066-10</div>
                                </div>
                            </div>
                            <div class="Invoice"
                                 style="background-color: var(--lm-ink);height: 40px;color: white;padding-top: 9px;padding-left: 10px;border: 1px solid var(--lm-border);margin-top: 10px;">
                                <span style="font-weight: 700; font-size: 16px">SALES TAX INVOICE</span>
                            </div>
                        </header>
                        <main>
                            <div class="row contacts">


                                <div class="col invoice-to">

                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Customer Name:</span> <p><?php echo $CardName; ?></p> </div>

                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">NTN #:</span> <p><?php echo ($VatIdUnCmp)?$VatIdUnCmp:'-'; ?></p> </div>
                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">STRN #:</span> <p><?php echo ($LicTradNum)?$LicTradNum:'-'; ?></p> </div>
                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Address:</span> <p><?php echo $address; ?></p> </div>

                                </div>
                                <div class="col invoice-details">

                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Sales Tax Invoice No :</span><p><?php echo ($U_fbr)?$U_fbr:'-'; ?></p></div>
                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Invoice Date :</span><p><?php echo $DocDate; ?></p> </div>
<!--                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Delivery Date:</span><p>--><?php //echo $DocDueDate; ?><!--</p> </div>-->
                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Customer PO:</span><p><?php echo ($NumAtCard)?$NumAtCard:'-'; ?></p> </div>
<!--                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Sale Order No:</span><p>--><?php //echo ($Sales_doc_no)?$Sales_doc_no:'-'; ?><!--</p> </div>-->
                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Credit Days:</span><p><?php echo ($GroupNum)?$GroupNum:'-'; ?></p> </div>
                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Delivery No:</span><p><?php echo ($Delivery_doc_no)?$Delivery_doc_no:'-'; ?></p> </div>
<!--                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Inquriy No:</span>-->
<!--                                        --->
<!--                                    </div>-->
<!--                                    <div class="text-gray-light"><span class="master-span" style="color: var(--lm-ink)">Inquiry Date:</span>-->
<!--                                        --->
<!--                                    </div>-->

                                </div>
                            </div>
                            <table  cellspacing="0" cellpadding="0">
                                <thead>
                                <tr>
                                    <th class="text-center">SR</th>
                                    <th class="text-center">Description</th>
                                    <th class="text-center">Unit</th>
                                    <th class="text-center">QTY</th>
                                    <th class="text-center">Rate</th>
                                    <th class="text-center">Amount</th>
                                    <th class="text-center">Sales Tax 18%</th>
                                    <th class="text-center">Furthur Tax Amount</th>
                                    <th class="text-center ">Net Amount</th>
                                </tr>
                                </thead>
                                <tbody style="height: 40px">
                                <?php
                                $totalqty = array();
                                $totalRate = array();
                                $totalTax = array();
                                $totalAmount = array();
                                $count = 1;
                                foreach ($lines as $line) { ?>

                                    <tr>
                                        <td class="rowtd"><?php echo $count; ?></td>

                                        <td class="rowtd"><?php echo $line['Dscription']; ?></td>
                                        <td class="rowtd"><?php echo $line['UomCode']; ?></td>
                                        <td class="rowtd"><?php echo $line['Quantity'];
                                            $totalqty[] = $line['Quantity'];
                                            ?></td>
                                        <td class="rowtd"><?php echo number_format($line['Price'], 2, '.', ',');
                                            $totalRate[] = $line['Price'];
                                            ?></td>
                                        <td class="rowtd"><?php
                                            echo number_format($line['PriceBefDi'] * $line['Quantity'], 2, '.', ',');
                                              $totalAmount[] = $line['PriceBefDi'] * $line['Quantity'];
                                            ?></td>
                                        <td class="rowtd"><?php

                                            $taxRate = 0;
                                            foreach ($_SESSION['tax'] as $tax) {

                                                if ($line['VatGroup'] == $tax['Code']) {
//                                                print_r($tax['Rate']);exit;

                                                    echo   number_format( ($tax['Rate']/ 100) * ($line['Price'] * $line['Quantity']), 2, '.', ',');
                                                     $totalTax[] = ($tax['Rate']/ 100) * ($line['Price'] * $line['Quantity']) ;

                                                     $taxRate = ($tax['Rate']/ 100) * ($line['Price'] * $line['Quantity']);
                                                }
                                            }
                                            //                                        echo $tax['Rate'];
                                            ?></td>
                                        <td  class="rowtd">0.00</td>
                                        <td class="rowtd"><?php

                                            echo number_format(($line['Price'] * $line['Quantity']) + $taxRate, 2, '.', ',');
//                                            echo ($line['Price'] * $line['Quantity']) + ($taxRate / 100) * ($line['Price'] * $line['Quantity'])
                                            ?></td>
                                    </tr>
                                <?php $count++; } ?>

                                </tbody>
                                <tfoot>

                                <tr>
                                    <td class="text-left"></td>
                                    <td class="text-center">Total</td>
                                    <td class="text-center"></td>

                                    <td class="text-center"><?php echo array_sum($totalqty); ?></td>
                                    <td class="text-center"></td>

                                    <td class="text-center"><?php echo number_format(array_sum($totalAmount), 2, '.', ','); ?></td>
                                    <td class="text-center"><?php echo number_format(array_sum($totalTax), 2, '.', ','); ?></td>
                                    <td class="text-center">0.00</td>

                                    <td class="text-center" ><?php echo number_format($DocTotal, 2, '.', ','); ?></td>
                                    <?php

                                    //                                    $f = new NumberFormatter("en", NumberFormatter::SPELLOUT);
                                    //                                    echo $f->format(1432);
                                    //                                    $amountInWords = $f->format(1432);
                                    $amount = $DocTotal; // Replace with your desired amount
                                    $amountInWords = convertNumberToWord($amount);


                                    ?>
                                </tr>

                                </tfoot>
                            </table>
                            <div class="notices">
                                <!--                                <div>NOTICE:</div>-->
                                <div class="notice"><span
                                        style="margin-right: 30px;color: var(--lm-ink)">Amount in Words :</span><?php echo $amountInWords; ?> Only
                                </div>
                            </div>
                            <div class="notices" style="margin-top: 20px">
                                <!--                                <div>NOTICE:</div>-->
                                <div class="notice"><span >Terms & Conditions</span><br>
                                    <p style="font-size: 13px;font-weight:400 ">Please make payments through means of Cheque in favor of "OASIS PACKAGING INDUSTRIES PVT LTD" .</p>
                                    <p style="font-size: 13px;font-weight:400 ">F.B.R. Discription : 39-b-Plastic Packaging Material Incl Boxes, Bags, Bottles.</p>
                                </div>
                            </div>

                            <div style="width: 25%; float: left;margin-top: 80px">
                                <hr>
                                <div>
                                    <span style="margin-left: 75px; color: black;font-size: 14px;font-weight: 600">Stamp & Signature</span>
                                </div>
                            </div>
                        </main>
                        <footer style=" position: fixed; left: 0;bottom: 20px; width: 100%;">


                        </footer>

                        <!--            <div class="Invoice" style="background-color: var(--lm-ink);height: 30px;color: white;padding-top: 35px;padding-left: 10px">-->
                        <!--                <span style="font-weight: 600">Invoice</span>-->
                        <!--            </div>-->

                    </div>
                    <!--DO NOT DELETE THIS div. IT is responsible for showing footer always at the bottom-->
                    <div></div>
                    <div id="myButton" class="toolbar hidden-print">
                        <div class="text-right">
                            <button id="printInvoice" class="btn btn-info"><i class="fa fa-print"></i> Print</button>
                        </div>
                        <hr>
                    </div>
                </div>
            </div>

        </div>
    </div>
</div>
<script>
    $('#printInvoice').click(function () {
        Popup($('.invoice')[0].outerHTML);
        function Popup(data) {
            window.print();
            return true;
        }
    });
</script>