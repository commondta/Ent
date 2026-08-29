<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class LandProvider extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('land_providers', function (Blueprint $table) {
            $table->id();
            $table->string('lp_cod');
            $table->string('doc_no');
            $table->string('lp_name');
            $table->string('exemption_decimals');
            $table->string('lp_cnic');
            $table->string('contact_no');
            $table->string('address');
            $table->string('attachments')->nullable();
            $table->string('ntn_no');
            $table->integer('status')->default(1);
            $table->string('security_deposited');
            $table->string('picture')->nullable();
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
