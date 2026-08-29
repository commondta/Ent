<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class PictorialView extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('pictorial_views', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->integer('pc_no');
            $table->string('lo_name');
            $table->string('chak');
            $table->string('lp_name');
            $table->string('area');
            $table->string('name_of_patwari');
            $table->string('kanal');
            $table->string('possession_jco');
            $table->string('marla');
            $table->string('signature1');
            $table->string('signature2');
            $table->string('picture');
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
