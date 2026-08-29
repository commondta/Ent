<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class SellerProfile extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {

        Schema::create('Seller_profiles', function (Blueprint $table) {
            $table->id();
            $table->string('lo_cod');
            $table->string('doc_no');
            $table->string('lo_name');
            $table->string('lo_father_name');
            $table->string('mouza');
            $table->string('rectangle');
            $table->string('khasra');
            $table->string('muraba');
            $table->string('marla');
            $table->string('kanal');
            $table->string('sq_feet');
            $table->string('lp_code');
            $table->string('lp_name')->default('');
            $table->string('lo_cnic');
            $table->string('attachment');
            $table->string('contact_no');
            $table->integer('status')->default(1);
            $table->string('address');
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
