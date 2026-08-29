<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class ExemptionForm extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('exemption_forms', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->integer('base_doc_no');
            $table->integer('file_no');
            $table->string('lo_name');
            $table->string('lo_code');
            $table->string('lp_name');
            $table->string('so');
            $table->string('reg_no');
            $table->string('mouza');
            $table->string('reg_date');
            $table->string('marla');
            $table->string('exemption_rate');
            $table->string('sq_feet');
            $table->string('total_files');
            $table->string('kanal');
            $table->string('khewat');
            $table->string('qatat');
            $table->string('khatooni');
            $table->string('file_security');
            $table->string('balance');
            $table->string('designation');
            $table->string('remarks');
            $table->string('signature');
            $table->string('attachment');
            $table->string('transfer_of_decimals');
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
