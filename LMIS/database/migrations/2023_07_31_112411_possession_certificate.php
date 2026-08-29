<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class PossessionCertificate extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('possession_certificates', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->string('base_code_no');
            $table->string('lo_name');
            $table->string('lo_father_name')->default(null);
            $table->string('contact_no');
            $table->string('address');
            $table->string('cnic');
            $table->string('cast');
            $table->string('signature');
            $table->string('sq_feet');
            $table->string('marla');
            $table->string('kanal');
            $table->string('mouza');
            $table->string('chak');
            $table->string('possession_date');
            $table->string('possession_khewat_NO');
            $table->string('possession_mustatil_no');
            $table->string('possession_muraba_no');
            $table->string('possession_khasra_no');
            $table->string('possession_kanal');
            $table->string('possession_marla');
            $table->string('possession_sq_feet');
            $table->string('lp_name');
            $table->string('lp_mobile_no');
            $table->string('lp_contact_no');
            $table->string('lp_signature1');
            $table->string('lp_rep_name');
            $table->string('lp_signature2');
            $table->string('lp_possession_jpo');
            $table->string('lp_signature3');
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
