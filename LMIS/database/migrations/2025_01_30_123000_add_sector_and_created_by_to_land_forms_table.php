<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddSectorAndCreatedByToLandFormsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('Land_forms', function (Blueprint $table) {
            // Add sector column if it doesn't exist
            if (!Schema::hasColumn('Land_forms', 'sector')) {
                $table->string('sector')->nullable()->after('mouza');
            }
            
            // Add createdBy column if it doesn't exist
            if (!Schema::hasColumn('Land_forms', 'createdBy')) {
                $table->unsignedBigInteger('createdBy')->nullable()->after('status');
            }
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::table('Land_forms', function (Blueprint $table) {
            if (Schema::hasColumn('Land_forms', 'sector')) {
                $table->dropColumn('sector');
            }
            
            if (Schema::hasColumn('Land_forms', 'createdBy')) {
                $table->dropColumn('createdBy');
            }
        });
    }
}
