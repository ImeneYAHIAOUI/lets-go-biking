
package Client.generated;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour anonymous complex type.
 * 
 * <p>Le fragment de sch�ma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="UdateItineraryOnTheMQResult" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "udateItineraryOnTheMQResult"
})
@XmlRootElement(name = "UdateItineraryOnTheMQResponse")
public class UdateItineraryOnTheMQResponse {

    @XmlElementRef(name = "UdateItineraryOnTheMQResult", namespace = "http://tempuri.org/", type = JAXBElement.class, required = false)
    protected JAXBElement<String> udateItineraryOnTheMQResult;

    /**
     * Obtient la valeur de la propri�t� udateItineraryOnTheMQResult.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getUdateItineraryOnTheMQResult() {
        return udateItineraryOnTheMQResult;
    }

    /**
     * D�finit la valeur de la propri�t� udateItineraryOnTheMQResult.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setUdateItineraryOnTheMQResult(JAXBElement<String> value) {
        this.udateItineraryOnTheMQResult = value;
    }

}
