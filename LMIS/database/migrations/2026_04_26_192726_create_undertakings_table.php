<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateUndertakingsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('undertakings', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->string('base_doc_no');
            $table->bigInteger('createdBy')->unsigned()->nullable();
            $table->tinyInteger('isDeleted')->default(0);
            $table->integer('status')->default(1);
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
        Schema::dropIfExists('undertakings');
    }
}
