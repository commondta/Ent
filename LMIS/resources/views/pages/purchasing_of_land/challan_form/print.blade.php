
<style>
    @media print {
        @page {
            size: A4;
            margin-top: 50px; /* Adjust the top margin as needed */
        }
        .page-break {
            page-break-before: always;
        }


        body {
            margin: 20px 70px;
            text-align: justify;
        }

        .btn {
            display: none;
        }
    }

    body {
        text-align: justify;
        margin: 12px 50px;
    }

    hr {
        border-top: 1px solid var(--lm-border);
    }

    .text_bold {
        font-weight: bold;
    }

    .abbrivation {
        font-size: 12px;
        vertical-align: top;
    }

    .heading {
        border-bottom: 1px solid var(--lm-border);
        display: inline-block;
    }

    .center {
        text-align: center;
    }

    .text_indent {
        text-indent: 30px;
    }

    .li_align {
        padding-left: 17px;
    }

    .flex_display {
        display: flex;
        text-align: justify;
    }

    .left_flex {
        width: 50%;
        margin-right: 30px;
    }

    .right_flex {
        width: 50%;
        margin-left: 30px;
    }

    .btn {
        background-color: blue;
        border: none;
        color: white;
        border-radius: 5px;
        padding: 10px 20px;;
        font-size: 16px;
        font-weight: bold;
        cursor: pointer;
        float: right;
        margin-bottom: 8px;
    }
    p{
        /*text-align: center;*/
    }

</style>
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
                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                        <div class="card-body">
                            <div class="d-flex align-items-center"><img src="{{ asset('public/assets/img/icons/logo.png'); }}"
                                                                        alt="phoenix"
                                                                        width="200"/>
                                <div class="center">
                                    <h1 class="heading">Challan Form</h1>
                                </div>
                                <div style="width: 100%;float: left">
                                    <div style="width: 40%;float: left;margin-left: 3%">
                                        <p><span style="font-weight: 700">Challan NO : </span> <span> {{$header->challan_no}}</span></p>
                                        <hr>
                                        <p><span style="font-weight: 700">Seller Name : </span> <span> {{$header->seller_name}}</span></p>
                                        <hr>

                                    </div>
                                    <div style="width: 40%;float: right;margin-left: 10%">
                                        <p><span style="font-weight: 700">Challan Date : </span> <span> {{ date('Y-m-d', strtotime($header->date)) }}</span></p>
                                        <hr>

                                        <p><span style="font-weight: 700">Seller CNIC : </span> <span> {{ date('Y-m-d', strtotime($header->seller_cnic)) }}</span></p>
                                        <hr>

                                    </div>


                                </div>
                                <table id="customers" style="margin-top: 30px">
                                    <tr>
                                        <th>Sr #</th>
                                        <th>Challan Type</th>
                                        <th>Amount</th>
                                    </tr>
                                    <?php $count = 1; ?>

                                    @foreach($header->rows as $row_data)
                                    <tr>
                                        <td>{{ $count }}</td>
                                        <td>{{ $row_data['challan_type'] }}</td>
                                        <td>{{ $row_data['amount'] }}</td>
                                    </tr>
                                        <?php $count++; ?>

                                    @endforeach

                                </table>
                                <div style="width: 100%;float: right">
                                    <button class="btn" onclick="window.print()">Print</button>

                                </div>









                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
    <script>
        window.onload = function() { window.print(); }
    </script>