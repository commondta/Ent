<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddPowerOfAttorneyToLandFormsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('Land_forms', function (Blueprint $table) {
            // Add Power of Attorney fields if they don't exist
            if (!Schema::hasColumn('Land_forms', 'poa_name')) {
                $table->string('poa_name')->nullable()->after('marla');
            }
            
            if (!Schema::hasColumn('Land_forms', 'poa_father_name')) {
                $table->string('poa_father_name')->nullable()->after('poa_name');
            }
            
            if (!Schema::hasColumn('Land_forms', 'poa_cnic')) {
                $table->string('poa_cnic')->nullable()->after('poa_father_name');
            }
            
            if (!Schema::hasColumn('Land_forms', 'poa_current_address')) {
                $table->text('poa_current_address')->nullable()->after('poa_cnic');
            }
            
            if (!Schema::hasColumn('Land_forms', 'poa_permanent_address')) {
                $table->text('poa_permanent_address')->nullable()->after('poa_current_address');
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
            if (Schema::hasColumn('Land_forms', 'poa_name')) {
                $table->dropColumn('poa_name');
            }
            
            if (Schema::hasColumn('Land_forms', 'poa_father_name')) {
                $table->dropColumn('poa_father_name');
            }
            
            if (Schema::hasColumn('Land_forms', 'poa_cnic')) {
                $table->dropColumn('poa_cnic');
            }
            
            if (Schema::hasColumn('Land_forms', 'poa_current_address')) {
                $table->dropColumn('poa_current_address');
            }
            
            if (Schema::hasColumn('Land_forms', 'poa_permanent_address')) {
                $table->dropColumn('poa_permanent_address');
            }
        });
    }
}
