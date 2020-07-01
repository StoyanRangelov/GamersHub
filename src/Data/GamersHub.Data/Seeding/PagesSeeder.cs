namespace GamersHub.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data.Models;

    public class PagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Pages.Any())
            {
                return;
            }

            await dbContext.Pages.AddAsync(new Page
            {
                Name = "Privacy",
                Content = @"<p>Welcome to <strong> GamersHub.</strong></p>
                            <p>Our Privacy Policy governs your visit to <strong>GamersHub</strong>, and explains how we collect, safeguard and disclose information that results from your use of our Service.</p>
                            <p>We use your data to provide and improve GamersHub. By using GamersHub, you agree to the collection and use of information in accordance with this policy.</p>
                            <p>We collect several different types of information for various purposes to provide and improve our Service to you.</p>
                            <p>While using our Service, we may ask you to provide us with certain personally identifiable information that can be used to contact or identify you. Personally identifiable information may include, but is not limited to:</p>
                            <p>1. Email address</p>
                            <p>2. First name and last name</p>
                            <p>3. Phone number</p>
                            <p>4. Cookies and Usage Data</p>
                            <p><strong>Usage Data</strong></p>
                            <p>We may also collect information that your browser sends whenever you visit our Service or when you access GamersHub by or through any device.</p>
                            <p>This Usage Data may include information such as your computer&rsquo;s Internet Protocol address (e.g. IP address), browser type, browser version, the pages of our Service that you visit, the time and date of your visit, the time spent on those pages, unique device identifiers and other diagnostic data.</p>
                            <p>When you access GamersHub with a device, this Usage Data may include information such as the type of device you use, your device unique ID, the IP address of your device, your device operating system, the type of Internet browser you use, unique device identifiers and other diagnostic data.</p>
                            <p><strong>Tracking Cookies Data</strong></p>
                            <p>We use cookies and similar tracking technologies to track the activity on our Service and we hold certain information.</p>
                            <p>Cookies are files with a small amount of data which may include an anonymous unique identifier. Cookies are sent to your browser from a website and stored on your device. Other tracking technologies are also used such as beacons, tags and scripts to collect and track information and to improve and analyze our Service.</p>
                            <p>You can instruct your browser to refuse all cookies or to indicate when a cookie is being sent. However, if you do not accept cookies, you may not be able to use some portions of our Service.</p>
                            <p>Examples of Cookies we use:</p>
                            <p>1. <strong>Session Cookies:</strong> We use Session Cookies to operate our Service.</p>
                            <p>2. <strong>Preference Cookies:</strong> We use Preference Cookies to remember your preferences and various settings.</p>
                            <p>3. <strong>Security Cookies:</strong> We use Security Cookies for security purposes.</p>
                            <p>4. <strong>Advertising Cookies:</strong> Advertising Cookies are used to serve you with advertisements that may be relevant to you and your interests.</p>
                            <p><strong>Use of Data</strong></p>
                            <p>GamersHub uses the collected data for various purposes:</p>
                            <p>1. to provide and maintain our Service.</p>
                            <p>2. to notify you about changes to our Service.</p>
                            <p>3. to allow you to participate in interactive features of our Service when you choose to do so.</p>
                            <p>4. to provide customer support.</p>
                            <p>5. to gather analysis or valuable information so that we can improve our Service.</p>
                            <p>6. to monitor the usage of our Service.</p>
                            <p>7. to detect, prevent and address technical issues.</p>
                            <p>8. to fulfil any other purpose for which you provide it.</p>
                            <p>9. in any other way we may describe when you provide the information.</p>
                            <p>10. for any other purpose with your consent.</p>
                            <p><strong>Security of Data</strong></p>
                            <p>The security of your data is important to us but remember that no method of transmission over the Internet or method of electronic storage is 100% secure. While we strive to use commercially acceptable means to protect your Personal Data, we cannot guarantee its absolute security.</p>
                            <p><strong>Changes to This Privacy Policy</strong></p>
                            <p>We may update our Privacy Policy from time to time. We will notify you of any changes by posting the new Privacy Policy on this page.</p>
                            <p>We will let you know via email and/or a prominent notice on our Service, prior to the change becoming effective.</p>
                            <p>You are advised to review this Privacy Policy periodically for any changes. Changes to this Privacy Policy are effective when they are posted on this page.</p>",
            });
        }
    }
}
