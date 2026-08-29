<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Land_provider extends Model
{
    use HasFactory;
    protected $fillable = ['lp_cod', 'doc_no', 'lp_name', 'relationship', 'lp_cnic', 'contact_no', 'address', 'tem_address', 'ntn_no', 'father_name', 'security_deposited', 'attachments', 'cnic_front_attachments', 'cnic_back_attachments', 'createdBy', 'status', 'isDeleted'];
}

