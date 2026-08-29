<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class Affidavit2 extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('affidavit_2s', function (Blueprint $table) {
            $table->id();
            $table->string('file_no');
            $table->string('doc_no');
            $table->date('date');
            $table->integer('base_doc_no');
            $table->string('lo_code');
            $table->string('lo_name');
            $table->string('lp_name');
            $table->string('lo_cnic');
            $table->string('lp_cnic');
            $table->string('lo_address');
            $table->string('mouza');
            $table->string('kanal');
            $table->string('code_no');
            $table->string('khewat');
            $table->string('marla');
            $table->string('qatat');
            $table->string('khatooni');
            $table->string('sale_deed_doc_no');
            $table->string('fard_id_no');
            $table->string('sale_deed_date');
            $table->integer('status')->default(1);
            $table->string('fard_id_date');
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
