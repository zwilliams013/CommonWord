package com.example.ryana.verticleprototype;

import android.content.Intent;

import android.graphics.Color;
import android.nfc.Tag;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.CalendarView;
import android.widget.TextView;
import java.util.Calendar;


public class Calendar extends AppCompatActivity {

    private CalendarView mCalendarView;

    private static final String TAG = "Calendar";
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.calendar);
        Intent intent = getIntent();
        getWindow().getDecorView().setBackgroundColor(Color.LTGRAY);

        mCalendarView = (CalendarView) findViewById(R.id.calendarView);

        mCalendarView.setOnDateChangeListener(new CalendarView.OnDateChangeListener() {
            @Override
            public void onSelectedDayChange(@NonNull CalendarView calendarView, int i, int i1, int i2) {
                String date = (i1 +1) +  "/" + i2 + "/" + i;
                Log.d(TAG, "onSelectedDayChange: mm/dd/yyyy " + date);
                TextView dateText = (TextView) findViewById(R.id.calendarDate);
                dateText.setText(date);


                Calendar calendar =  java.util.Calendar.getInstance();

                calendar.set(year, month, dayOfMonth);
                int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);


            }
        });
    }


}