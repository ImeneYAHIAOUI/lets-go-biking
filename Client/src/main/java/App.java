import Client.generated.*;
import org.jxmapviewer.viewer.GeoPosition;

import javax.imageio.ImageIO;
import javax.jms.JMSException;
import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
//import java.lang.invoke.DelegatingMethodHandle$Holder;


public class App {
    public static void main(String[] args) throws IOException {

        JFrame frame = new JFrame(); //JFrame Creation
        frame.setTitle("Let's GO BIKING"); //Add the title to frame
        frame.getContentPane().setBackground(Color.WHITE);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE); //Terminate program on close button
        frame.setSize(800, 350);
        frame.setLayout( new FlowLayout(FlowLayout.CENTER, 20, 0));

        JPanel originPanel = new JPanel(new GridBagLayout());
        originPanel.setBackground(Color.WHITE);
        originPanel.setLayout(new BoxLayout(originPanel, BoxLayout.Y_AXIS));

        JPanel destPanel = new JPanel(new GridBagLayout());
        destPanel.setBackground(Color.WHITE);
        destPanel.setLayout(new BoxLayout(destPanel, BoxLayout.Y_AXIS));

        //text fields
        JLabel originAddressLabel = new JLabel("<html> <font color='red'>Origin address</font></html>");
        originPanel.add(originAddressLabel);

        JLabel originStreetLabel = new JLabel("Street name and number");
        originPanel.add(originStreetLabel);
        JTextField originStreetText = new JTextField(1);
        originPanel.add(originStreetText);

        JLabel originCityLabel = new JLabel("City");
        originPanel.add(originCityLabel);
        JTextField originCityText = new JTextField(1);
        originPanel.add(originCityText);

        JLabel originZipLabel = new JLabel("Zip code");
        originPanel.add(originZipLabel);
        JTextField originZipText = new JTextField(1);
        originPanel.add(originZipText);

        JLabel destinationAddressLabel = new JLabel("<html><font color='red'>Destination address</font></html>");
        destPanel.add(destinationAddressLabel);
        JLabel destStreetLabel = new JLabel("Street name and number");
        destPanel.add(destStreetLabel);
        JTextField destStreetText = new JTextField(1);
        destPanel.add(destStreetText);

        JLabel destCityLabel = new JLabel("City");
        destPanel.add(destCityLabel);
        JTextField destCityText = new JTextField(1);
        destPanel.add(destCityText);

        JLabel destZipLabel = new JLabel("Zip code");
        destPanel.add(destZipLabel);
        JTextField destZipText = new JTextField(1);
        destPanel.add(destZipText);


        //BUTTON
        JButton startBtn = new JButton("Click to get itinerary");
        //panel.add(startBtn);
        // add logo
        JLabel label = new JLabel(); //JLabel Creation
        BufferedImage bufferedImage = ImageIO.read(new File(System.getProperty("user.dir")+"\\src\\main\\java\\images\\logo.png"));
        Image image = bufferedImage.getScaledInstance(413, 213, Image.SCALE_DEFAULT);
        ImageIcon icon = new ImageIcon(image);
        label.setIcon(icon);
        label.setBounds (0, 0,icon.getIconWidth()+20,icon.getIconHeight()+20); //Sets the location of the image
        //Adds logo to the container
        frame.add(label,BorderLayout.SOUTH);
        frame.add(originPanel);
        frame.add(destPanel);
        frame.add(startBtn,  Component.CENTER_ALIGNMENT);
        Dimension dimension = Toolkit.getDefaultToolkit().getScreenSize();
        int x = (int) ((dimension.getWidth() - frame.getWidth()) / 2);
        int y = (int) ((dimension.getHeight() - frame.getHeight()) / 2);
        frame.setLocation(x, y);
        frame.setVisible(true); // Exhibit the frame

        startBtn.addActionListener(e -> {
            if (originStreetText.getText().isEmpty() || originCityText.getText().isEmpty() || originZipText.getText().isEmpty() || destStreetText.getText().isEmpty() || destCityText.getText().isEmpty() || destZipText.getText().isEmpty()) {
                JOptionPane.showMessageDialog(frame, "Please fill in all fields");
            }
            else {
                String originAddress = originStreetText.getText() + ", " + originCityText.getText()  +" "+ originZipText.getText();
                String destinationAddress = destStreetText.getText() + ", " + destCityText.getText() +" "+ destZipText.getText();

                try {
                    LetsGoBiking letsGoBiking = new LetsGoBiking();
                    ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();

                    ArrayOfItinerary itineraries = proxy.getItinerary(originAddress, destinationAddress);

                    if (itineraries.getItinerary().size() == 0) {
                        JOptionPane.showMessageDialog(frame, "No itineraries were found, make sure you entered valid addresses.\n If the problem is not resolved, it may be an internet connection problem on the server side");
                    } else {

                        try{
                            String queueName = proxy.getItineraryOntheMQ(originAddress, destinationAddress);
                            new Receiver(queueName)
                                    .configurer();
                        }catch(JMSException exception){
                            System.out.println("error, cannot connect to the MQ");
                        }
                        getItinerary(itineraries,originAddress,destinationAddress);
                    }
                } catch (Exception exception) {
                    JOptionPane.showMessageDialog(frame, "Error while getting itinerary, the server may be down");
                }
            }
        });
    }
    public static void getItinerary(ArrayOfItinerary itineraries,String originAddress, String destinationAddress) throws IOException {
        List<List<GeoPosition>> geoList = new ArrayList<>();
        List<String> steps = new ArrayList<>();
        double totalDistance = 0;
        for (Itinerary itinerary : itineraries.getItinerary()) {
            totalDistance += itinerary.getDistance();
            List<GeoPosition> geoPositions = new ArrayList<>();
            ArrayOfArrayOfdouble coordinates = itinerary.getGeometry().getValue().getCoordinates().getValue();
            for (ArrayOfdouble coordinate : coordinates.getArrayOfdouble()) {
                geoPositions.add(new GeoPosition(coordinate.getDouble().get(1), coordinate.getDouble().get(0)));
            }
            steps.add(itinerary.getProfile().value());
            for (Step step : itinerary.getSteps().getValue().getStep()) {
                String duration =secToTime((int)Math.floor(step.getDuration()));
                if(!duration.isEmpty()){
                    duration = " for " + duration;
                }
                else {
                    duration = "";
                }
                steps.add(step.getInstruction().getValue()+duration+" ("+step.getDistance()+"m)" );
            }
            geoList.add(geoPositions);

        }


        new ItineraryMap(geoList, originAddress, destinationAddress, steps, totalDistance);
    }
    static String secToTime(int sec) {
        int seconds = sec % 60;
        int minutes = sec / 60;
        if (minutes >= 60) {
            int hours = minutes / 60;
            minutes %= 60;
            if (hours >= 24) {
                int days = hours / 24;
                return String.format("%d days %02d:%02d:%02d", days, hours % 24, minutes, seconds);
            }
            return String.format("%d hour and %2d minutes", hours, minutes);
        }

        if(minutes > 0) {
            if (seconds >30)
                minutes++;
            return String.format("%2d minutes", minutes);
        }

        return "";
    }
}