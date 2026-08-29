<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddSellerProfileFieldsToLandFormRowsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('land_form_rows', function (Blueprint $table) {
            if (!Schema::hasColumn('land_form_rows', 'lo_father_name')) {
                $table->string('lo_father_name')->nullable()->after('lo_name');
            }
            
            if (!Schema::hasColumn('land_form_rows', 'address')) {
                $table->text('address')->nullable()->after('contact_no');
            }
            
            if (!Schema::hasColumn('land_form_rows', 'tem_address')) {
                $table->text('tem_address')->nullable()->after('address');
            }
            
            if (!Schema::hasColumn('land_form_rows', 'land_category')) {
                $table->string('land_category')->nullable()->after('tem_address');
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
        Schema::table('land_form_rows', function (Blueprint $table) {
            if (Schema::hasColumn('land_form_rows', 'lo_father_name')) {
                $table->dropColumn('lo_father_name');
            }
            
            if (Schema::hasColumn('land_form_rows', 'address')) {
                $table->dropColumn('address');
            }
            
            if (Schema::hasColumn('land_form_rows', 'tem_address')) {
                $table->dropColumn('tem_address');
            }
            
            if (Schema::hasColumn('land_form_rows', 'land_category')) {
                $table->dropColumn('land_category');
            }
        });
    }
}
