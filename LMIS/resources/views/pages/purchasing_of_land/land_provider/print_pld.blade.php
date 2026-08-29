@extends('layouts.main')

@section('content')

    <style>
        #customers {
            font-family: Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        #customers td, #customers th {
            border: 1px solid var(--lm-border);
            padding: 8px;
        }

        #customers tr:nth-child(even){background-color: var(--lm-surface);}

        #customers tr:hover {background-color: var(--lm-border);}

        #customers th {
            padding-top: 12px;
            padding-bottom: 12px;
            text-align: left;
            background-color: var(--lm-ink);
            color: white;
        }
    </style>
    <div class="content">
        <div class="mt-4">
            <div class="row g-4">
                <div class="col-12 col-xl-12 order-1 order-xl-0">
                    <div class="mb-9">
                        @if(session('success'))

                            <div class="alert alert-outline-success d-flex align-items-center" role="alert">
                                <span class="fas fa-check-circle text-success fs-3 me-3"></span>
                                <p class="mb-0 flex-1">{{ session('success') }}</p>
                                <button class="btn-close" type="button" data-bs-dismiss="alert" aria-label="Close"></button>
                            </div>

                        @endif
                        @if(session('danger'))
                            <div class="alert alert-outline-danger d-flex align-items-center" role="alert">
                                <span class="fas fa-times-circle text-danger fs-3 me-3"></span>
                                <p class="mb-0 flex-1">{{ session('danger') }}</p>
                                <button class="btn-close" type="button" data-bs-dismiss="alert" aria-label="Close"></button>
                            </div>
                        @endif
                            <div class="invoice overflow-auto">

                            <header>
                                <div class="row" style="margin: 0">
                                    <div class="col">

                                        <img src="{{ asset('public/assets/img/icons/logo.png'); }}" alt="" height="60"
                                             data-holder-rendered="true">

                                    </div>

                                </div>
                                <div class="Invoice"
                                     style="text-align:center;margin-top: 10px;">
                                    <span style="font-weight: 700; font-size: 16px">Land Provider Master Data</span>
                                </div>
                            </header>
                            <table id="customers" class="table table-striped table-sm fs--1 mb-0">
                                <thead>
                                <tr>
                                    <th class="text-center">SR</th>
                                    <th class="text-center">Doc No</th>
                                    <th class="text-center">LP Name</th>
                                    <th class="text-center">LP CNIC</th>
                                    <th class="text-center">Address</th>
                                    <th class="text-center">Security Deposit</th>
                                    <th class="text-center">Contact No</th>
                                    <th class="text-center">NTN</th>
                                    <th class="text-center">Incorporation Date</th>
                                </tr>
                                </thead>
                                <tbody >
                                <?php

                                $count = 1;
                                ?>
                                @foreach($record as $row)

                                    <tr>
                                        <td ><?php echo $count; ?></td>

                                        <td >{{ $row->doc_no }}</td>
                                        <td >{{ $row->lp_name }}</td>
                                        <td >{{ $row->lp_cnic }}</td>
                                        <td >{{ $row->address }}</td>
                                        <td >{{ $row->security_deposited }}</td>
                                        <td >{{ $row->contact_no }}</td>
                                        <td >{{ $row->ntn_no }}</td>
                                        <td >{{ $row->created_at }}</td>

                                    </tr>
                                    <?php $count++; ?>
                                @endforeach

                                </tbody>
                            </table>
                                </div>
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
        <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 5">
            <div class="toast align-items-center text-white bg-dark border-0 light" id="icon-copied-toast" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body p-3"></div><button class="btn-close btn-close-white me-2 m-auto" type="button" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        </div>
        <footer class="footer position-absolute">
            <div class="row g-0 justify-content-between align-items-center h-100">
                <div class="col-12 col-sm-auto text-center">
                    <p class="mb-0 mt-2 mt-sm-0 lm-footer-text"><span class="lm-footer-brand">Land Information Management System</span><span class="lm-footer-sep">|</span><span>&copy; {{ date('Y') }}</span><span class="lm-footer-sep">|</span><span>Powered by <img src="{{ asset('public/assets/img/n-stack-logo.png') }}" alt="" class="lm-footer-logo"> <strong>N-Stack</strong></span></p>
                </div>
                <div class="col-12 col-sm-auto text-center">
                </div>
            </div>
        </footer>
    </div>
    <script>
        $('#printInvoice').click(function () {
            // Clone the invoice element to create a new element containing only the invoice content
            var invoiceContent = $('.invoice').clone();

            // Create a new window and append the cloned invoice content along with styles
            var popupWin = window.open('', '_blank');
            popupWin.document.open();
            popupWin.document.write('<html><head><title>Print</title>');
            // Include styles from the main document
            $('link[rel="stylesheet"]').each(function(){
                popupWin.document.write('<link rel="stylesheet" href="' + $(this).attr('href') + '">');
            });
            popupWin.document.write('</head><body>');
            popupWin.document.write(invoiceContent[0].outerHTML);
            popupWin.document.write('</body></html>');
            popupWin.document.close();

            // Trigger the print function in the new window
            popupWin.print();
            popupWin.onafterprint = function () {
                // Close the window after printing
                popupWin.close();
            };

            return true;
        });
    </script>


    {{--<script>--}}
        {{--$('#printInvoice').click(function () {--}}
            {{--Popup($('.invoice')[0].outerHTML);--}}
            {{--function Popup(data) {--}}
                {{--window.print();--}}
                {{--return true;--}}
            {{--}--}}
        {{--});--}}
    {{--</script>--}}
    <!-- Your content here -->
@endsection