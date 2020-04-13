package com.hkucsfyp.controller;


import android.bluetooth.BluetoothDevice;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

public class BluetoothDevListActivity extends AppCompatActivity
        implements ControllerService.ResultHandler, View.OnClickListener {
    static public final String EXTRA_KEY_ADDRESS = "address";
    static public final String EXTRA_KEY_DEVICE = "device";

    private RecyclerView mRecyclerView;
    private BluetoothDevRecyclerView.Adapter mAdapter;
    private boolean mIsScanning = false;
    private Button mButtonScan;
    private ControllerService controllerService;
    private BluetoothDevice selectedDevice;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_bluetooth_dev_list);

        controllerService = ControllerService.createInstance(this);
        //
        mButtonScan = findViewById(R.id.buttonBtScan);
        mButtonScan.setOnClickListener(this);
        // Get a handle to the RecyclerView.
        mRecyclerView = findViewById(R.id.recyclerview);
        mRecyclerView.setLayoutManager(new LinearLayoutManager(this));
        // Create an adapter and supply the data to be displayed.
        mAdapter = new BluetoothDevRecyclerView.Adapter(this);
        mRecyclerView.setAdapter(mAdapter);
        //
        controllerService.setResultHandler(this);
    }

    public void onClick(View v) {
        if (v.getId() == R.id.buttonBtScan) {
            if (!mIsScanning) {
                mAdapter.clearBluetoothDeviceList();
                mIsScanning=controllerService.startDiscovery();
            } else {
                controllerService.stopDiscovery();
                mIsScanning = false;
            }
            return;
        }
        //bluetooth device is selected from recycle view
        TextView textView = v.findViewById(R.id.deviceAddr);
        String address = textView.getText().toString();
        //
        selectedDevice = mAdapter.getBluetoothDevice(address);
        mAdapter.clearBluetoothDeviceList();
        //
        Intent intent = new Intent();
        intent.putExtra(EXTRA_KEY_ADDRESS, address);
        if (selectedDevice != null) {
            intent.putExtra(EXTRA_KEY_DEVICE, selectedDevice);
        }
        setResult(RESULT_OK, intent);
        finish();
    }
    @Override
    protected void onStop() {
        super.onStop();
        controllerService.stopDiscovery();
    }

    @Override
    public void setResult(BluetoothDevice bluetoothDevice) {
        mAdapter.addBluetoothDevice(bluetoothDevice);
    }

    @Override
    public void onPostResult() {
    }
}
