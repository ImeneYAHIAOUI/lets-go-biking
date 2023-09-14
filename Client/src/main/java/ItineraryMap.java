import Client.generated.ArrayOfItinerary;
import Client.generated.ILetsGoBiking;
import Client.generated.LetsGoBiking;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.OSMTileFactoryInfo;
import org.jxmapviewer.input.CenterMapListener;
import org.jxmapviewer.input.PanKeyListener;
import org.jxmapviewer.input.PanMouseInputListener;
import org.jxmapviewer.input.ZoomMouseWheelListenerCursor;
import org.jxmapviewer.painter.CompoundPainter;


import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import javax.imageio.ImageIO;
import javax.jms.JMSException;
import javax.swing.*;
import javax.swing.event.MouseInputListener;

import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.viewer.DefaultTileFactory;
import org.jxmapviewer.viewer.GeoPosition;
import org.jxmapviewer.viewer.TileFactoryInfo;
import org.jxmapviewer.viewer.WaypointPainter;
import swing.FancyWaypointRenderer;
import swing.MapAdapter;
import swing.MapPainter;
import swing.MyWaypoint;

/**
 * A simple sample application that shows
 * a OSM map of Europe containing a route with waypoints
 * @author Martin Steiger
 */
public class ItineraryMap {


    public ItineraryMap(List<List<GeoPosition>> tracks, String origin, String dest, List<String> steps, double distance) throws IOException {
        JXMapViewer mapViewer = new JXMapViewer();

        // Display the viewer in a JFrame
        JFrame frame = new JFrame("Itinerary : from " + origin + " to " + dest);
        frame.setLayout(new BorderLayout());
        frame.getContentPane().add(mapViewer, BorderLayout.CENTER);
        frame.setSize(1000, 600);
        frame.setVisible(true);
        Dimension dimension = Toolkit.getDefaultToolkit().getScreenSize();
        int x = (int) ((dimension.getWidth() - frame.getWidth()) / 2);
        int y = (int) ((dimension.getHeight() - frame.getHeight()) / 2);
        frame.setLocation(x, y);

        // Create a TileFactoryInfo for OpenStreetMap
        TileFactoryInfo info = new OSMTileFactoryInfo();
        DefaultTileFactory tileFactory = new DefaultTileFactory(info);
        mapViewer.setTileFactory(tileFactory);


        MouseInputListener mia = new PanMouseInputListener(mapViewer);
        mapViewer.addMouseListener(mia);
        mapViewer.addMouseMotionListener(mia);

        mapViewer.addMouseListener(new CenterMapListener(mapViewer));

        mapViewer.addMouseWheelListener(new ZoomMouseWheelListenerCursor(mapViewer));

        mapViewer.addKeyListener(new PanKeyListener(mapViewer));

        // Add a selection painter
        MapAdapter sa = new MapAdapter(mapViewer);
        mapViewer.addMouseListener(sa);
        mapViewer.addMouseMotionListener(sa);

        List<MyWaypoint> mwp = new ArrayList<>();
        int i = 0;
        MapPainter MapPainter;
        List<Painter<JXMapViewer>> painters = new ArrayList<Painter<JXMapViewer>>();

        for (List<GeoPosition> track : tracks) {

            if (i == 0) {
                mwp.add(new MyWaypoint(Color.CYAN, track.get(track.size() - 1)));
            }
            if (i == tracks.size() - 1)
                mwp.add(new MyWaypoint(Color.RED, track.get(0)));
            if (i % 2 == 0)
                MapPainter = new MapPainter(track);


            else {
                MapPainter = new MapPainter(track, Color.GREEN);
                mwp.add(new MyWaypoint(Color.GREEN, track.get(0)));
                mwp.add(new MyWaypoint(Color.GREEN, track.get(track.size() - 1)));
            }

            mapViewer.zoomToBestFit(new HashSet<>(track), 0.1);
            painters.add(MapPainter);
            i++;
        }


        // Create waypoints from the geo-positions
        Set<MyWaypoint> waypoints = new HashSet<>(mwp);
        // Create a waypoint painter that takes all the waypoints
        WaypointPainter<MyWaypoint> waypointPainter = new WaypointPainter<>();
        waypointPainter.setWaypoints(waypoints);
        waypointPainter.setRenderer(new FancyWaypointRenderer(ImageIO.read(new File(System.getProperty("user.dir") + "\\src\\main\\java\\images\\waypoint_white.png"))));


        mapViewer.setOverlayPainter(waypointPainter);
        // Create a compou nd painter that uses both the route-painter and the waypoint-painter

        painters.add(waypointPainter);

        CompoundPainter<JXMapViewer> painter = new CompoundPainter<JXMapViewer>(painters);
        mapViewer.setOverlayPainter(painter);

        JPanel southPanel = new JPanel(new GridBagLayout());
        southPanel.setLayout(new BoxLayout(southPanel, BoxLayout.X_AXIS));
        JPanel LegendPanel = new JPanel(new GridBagLayout());

        LegendPanel.setLayout(new BoxLayout(LegendPanel, BoxLayout.Y_AXIS));
        JLabel LegendLabel = new JLabel("Legend");
        LegendLabel.setFont(new Font("Serif", Font.BOLD, 20));
        LegendPanel.add(LegendLabel);
        LegendPanel.add(new JLabel("<html><font color='red'>red lines</font> : on foot itinerary</html>"));
        LegendPanel.add(new JLabel("<html><font color='green'>green lines</font> : by bike itinerary</html>"));
        LegendPanel.add(new JLabel("<html><font color='blue'>cyan way point</font> : origin</html>"));
        LegendPanel.add(new JLabel("<html><font color='red'>red way point</font> : destination</html>"));
        LegendPanel.add(new JLabel("<html><font color='green'>green way point</font> : destination</html>"));
        JPanel StepsPanel = new JPanel(new GridBagLayout());
        StepsPanel.setLayout(new BoxLayout(StepsPanel, BoxLayout.Y_AXIS));
        JLabel stepsLabel = new JLabel("Steps");
        stepsLabel.setFont(new Font("Serif", Font.BOLD, 20));
        StepsPanel.add(stepsLabel);
        String strDist;
        if (distance < 1000)
            strDist = String.format("%.2f", distance) + " m";
        else
            strDist = String.format("%.2f", distance / 1000) + " km";
        JLabel distanceLabel = new JLabel("total distance : " + strDist);
        distanceLabel.setFont(new Font("Serif", Font.BOLD, 15));
        StepsPanel.add(distanceLabel);
        for (String step : steps) {
            if (step.equals("Walk"))
                step = "<html><font color='red'>" + step + "</font></html>";
            if (step.equals("Cycle"))
                step = "<html><font color='green'>" + step + "</font></html>";

            StepsPanel.add(new JLabel(step));
        }

        JScrollPane scrollFrame = new JScrollPane(StepsPanel);
        JButton button = new JButton("Update itinerary");
        button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                JFrame updateFrame = new JFrame("Enter your location address");
                updateFrame.setSize(300, 300);
                updateFrame.setVisible(true);
                updateFrame.setLayout(new FlowLayout(FlowLayout.CENTER, 20, 0));
                Dimension dimension = Toolkit.getDefaultToolkit().getScreenSize();
                int x = (int) ((dimension.getWidth() - updateFrame.getWidth()) / 2);
                int y = (int) ((dimension.getHeight() - updateFrame.getHeight()) / 2);
                updateFrame.setLocation(x, y);
                updateFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
                JPanel panel = new JPanel(new GridBagLayout());
                panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
                updateFrame.add(panel, BorderLayout.CENTER);
                JLabel uAddressLabel = new JLabel("<html><font color='red'>Enter current location</font></html>");
                panel.add(uAddressLabel);
                JLabel uStreetLabel = new JLabel("Street name and number");
                panel.add(uStreetLabel);
                JTextField uStreetText = new JTextField(1);
                panel.add(uStreetText);


                JLabel uCityLabel = new JLabel("City");
                panel.add(uCityLabel);
                JTextField uCityText = new JTextField(1);
                panel.add(uCityText);

                JLabel uZipLabel = new JLabel("Zip code");
                panel.add(uZipLabel);
                JTextField uZipText = new JTextField(1);
                panel.add(uZipText);
                Button updateButton = new Button("Update");
                updateButton.setBackground(Color.cyan);
                updateFrame.add(updateButton, BorderLayout.SOUTH);

                updateButton.addActionListener(new ActionListener() {
                    @Override
                    public void actionPerformed(ActionEvent e) {

                        String uStreet = uStreetText.getText();
                        String uCity = uCityText.getText();
                        String uZip = uZipText.getText();
                        updateFrame.dispose();
                        String uAddress = uStreet + " ," + uCity + " " + uZip;
                        LetsGoBiking letsGoBiking = new LetsGoBiking();
                        ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();
                        ArrayOfItinerary itineraries = proxy.udateItinerary(uAddress, dest);
                        if (itineraries.getItinerary().size() == 0) {
                            JOptionPane.showMessageDialog(frame, "No itineraries were found, make sure you entered valid addresses");
                        } else {

                            try {

                                String queueName = proxy.udateItineraryOnTheMQ(uAddress, dest);
                                new Receiver(queueName)
                                        .configurer();
                            } catch (JMSException exception) {
                                System.out.println("error, cannot connect to the MQ");
                            }
                            try {
                                App.getItinerary(itineraries, uAddress, dest);
                                frame.dispose();
                            } catch (IOException ex) {
                                throw new RuntimeException(ex);
                            }


                        }


                    }
                });
            }
        });

        southPanel.add(LegendPanel);
        southPanel.add(button);
        frame.getContentPane().add(southPanel, BorderLayout.SOUTH);
        frame.getContentPane().add(scrollFrame, BorderLayout.WEST);
    }
}



