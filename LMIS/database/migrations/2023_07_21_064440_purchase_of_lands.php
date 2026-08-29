<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class PurchaseOfLands extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('Purchase_of_lands', function (Blueprint $table) {
            $table->id();
            $table->string('File_No');
            $table->date('doc_date');
            $table->string('land_form_no');
            $table->date('posting_date');
            $table->string('area');
            $table->string('lo_cod');
            $table->string('lo_name');
            $table->string('lp_name');
            $table->string('so');
            $table->string('amount');
            $table->string('mouza');
            $table->string('khatoni');
            $table->string('khewat_no');
            $table->string('rectangle');
            $table->string('khasra');
            $table->string('muraba');
            $table->string('acre');
            $table->string('kanal');
            $table->string('qatat');
            $table->string('sq_feet');
            $table->string('marla');
            $table->string('chak');
            $table->string('exemption_rate');
            $table->string('transferred_share');
            $table->string('attachment_nfc_sub_registrar');
            $table->string('attachment_aks_shajra');
            $table->string('attachment_girdwari');
            $table->string('attachment_fard_milkiyat');
            $table->string('attachment_khata_of_land');
            $table->string('designation');
            $table->string('remarks');
            $table->string('signature');
            $table->string('attachment');
            $table->integer('status')->default(1);
            $table->boolean('isDeleted')->default(0);
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        //
    }
}
