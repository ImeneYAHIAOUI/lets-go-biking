package swing;

/*
 * WaypointRenderer.java
 *
 * Created on March 30, 2006, 5:24 PM
 *
 * To change this template, choose Tools | Template Manager
 * and open the template in the editor.
 */

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.awt.image.BufferedImage;
import java.util.HashMap;
import java.util.Map;


import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.viewer.WaypointRenderer;

/**
 * A fancy waypoint painter
 * @author Martin Steiger
 */
public class FancyWaypointRenderer implements WaypointRenderer<MyWaypoint>
{

    private final Map<Color, BufferedImage> map = new HashMap<Color, BufferedImage>();

//    private final Font font = new Font("Lucida Sans", Font.BOLD, 10);

    private BufferedImage origImage;

    /**
     * Uses a default waypoint image
     */
    public FancyWaypointRenderer(BufferedImage img)
    {
        try
        {
            origImage  = img;

        }
        catch (Exception ex)
        {
            ex.printStackTrace();

        }
    }


    private BufferedImage convert(BufferedImage loadImg, Color newColor)
    {
        int w = loadImg.getWidth();
        int h = loadImg.getHeight();
        BufferedImage imgOut = new BufferedImage(w, h, BufferedImage.TYPE_INT_ARGB);
        BufferedImage imgColor = new BufferedImage(w, h, BufferedImage.TYPE_INT_ARGB);

        Graphics2D g = imgColor.createGraphics();
        g.setColor(newColor);
        g.fillRect(0, 0, w+1, h+1);
        g.dispose();

        Graphics2D graphics = imgOut.createGraphics();
        graphics.drawImage(loadImg, 0, 0, null);
        graphics.setComposite(MultiplyComposite.Default);
        graphics.drawImage(imgColor, 0, 0, null);
        graphics.dispose();

        return imgOut;
    }

    @Override
    public void paintWaypoint(Graphics2D g, JXMapViewer viewer, MyWaypoint w)
    {
        g = (Graphics2D)g.create();

        if (origImage == null)
            return;

        BufferedImage myImg = map.get(w.getColor());

        if (myImg == null)
        {
            myImg = convert(origImage, w.getColor());
            map.put(w.getColor(), myImg);
        }

        Point2D point = viewer.getTileFactory().geoToPixel(w.getPosition(), viewer.getZoom());

        int x = (int)point.getX();
        int y = (int)point.getY();

        g.drawImage(myImg, x -myImg.getWidth() / 2, y -myImg.getHeight(), null);


//        g.setFont(font);




        g.dispose();
    }
}