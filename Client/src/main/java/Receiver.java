import java.util.Hashtable;

import javax.jms.ConnectionFactory;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.Queue;
import javax.jms.TextMessage;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;

public class Receiver implements javax.jms.MessageListener{
    private javax.jms.Session receiveSession = null;
    InitialContext context = null;
    private javax.jms.Connection connect = null;
    private  String queueName;
    public Receiver(String queueName){
        this.queueName=queueName;
    }
    public void configurer() throws JMSException {
        try{

            Hashtable properties = new Hashtable();
            properties.put(Context.INITIAL_CONTEXT_FACTORY,
                    "org.apache.activemq.jndi.ActiveMQInitialContextFactory");
            properties.put(Context.PROVIDER_URL, "tcp://localhost:61616");
            context = new InitialContext(properties); // ou new InitialContext sans parametre si les options de JVM sont utilisées
            javax.jms.ConnectionFactory factory = (ConnectionFactory) context.lookup("ConnectionFactory");
            connect = factory.createConnection();
            this.configurerConsommateur();
            connect.start(); // on peut activer la connection.
        } catch (javax.jms.JMSException jmse){
            jmse.printStackTrace();
        } catch (NamingException e) {
            // TODO Auto-generated catch block
           e.printStackTrace();
        }
    }
    private void configurerConsommateur() throws JMSException, NamingException{
        // Pour consommer, il faudra simplement ouvrir une session
        receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        // et dire dans cette session quelle queue(s) et topic(s) on accèdera et dans quel mode
        Queue queue = (Queue) context.lookup("dynamicQueues/"+queueName);
        System.out.println("Nom de la queue " + queue.getQueueName());
        javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
        //MessageConsumer est typé en QueueReceiver puisque on a passé queue comme param.
        qReceiver.setMessageListener(this);
    }
    @Override
    public void onMessage(Message message) {
        try {
            System.out.println(((TextMessage)message).getText());
        } catch (JMSException e) {
            throw new RuntimeException(e);
        }
    }
}
