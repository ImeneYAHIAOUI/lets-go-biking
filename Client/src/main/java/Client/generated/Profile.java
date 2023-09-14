
package Client.generated;

import javax.xml.bind.annotation.XmlEnum;
import javax.xml.bind.annotation.XmlEnumValue;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour Profile.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * <p>
 * <pre>
 * &lt;simpleType name="Profile"&gt;
 *   &lt;restriction base="{http://www.w3.org/2001/XMLSchema}string"&gt;
 *     &lt;enumeration value="Cycle"/&gt;
 *     &lt;enumeration value="Walk"/&gt;
 *   &lt;/restriction&gt;
 * &lt;/simpleType&gt;
 * </pre>
 * 
 */
@XmlType(name = "Profile", namespace = "http://schemas.datacontract.org/2004/07/SelfRootingServer.OpenRouteService")
@XmlEnum
public enum Profile {

    @XmlEnumValue("Cycle")
    CYCLE("Cycle"),
    @XmlEnumValue("Walk")
    WALK("Walk");
    private final String value;

    Profile(String v) {
        value = v;
    }

    public String value() {
        return value;
    }

    public static Profile fromValue(String v) {
        for (Profile c: Profile.values()) {
            if (c.value.equals(v)) {
                return c;
            }
        }
        throw new IllegalArgumentException(v);
    }

}
