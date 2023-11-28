using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace postcard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage[] girl; // Масив для зберігання кадрів анімації
        private BitmapImage[] skate; // Масив для зберігання кадрів анімації
        private BitmapImage[] box; // Масив для зберігання кадрів анімації
        private int currentFrameIndexGirl = 0; // Індекс поточного кадру
        private int currentFrameIndexSkate = 0; // Індекс поточного кадру
        private int currentFrameIndexBox = 0; // Індекс поточного кадру
        private int repeatCount = 0; // Індекс поточного кадру
        private DispatcherTimer timer;
        private DispatcherTimer timerBox;
        private bool isAnimationRunning = false;


        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick;


            // Завантаження всіх кадрів анімації при ініціалізації форми
            LoadFrames();

            if ((girl != null && girl.Length > 0) && (skate != null && skate.Length > 0))
            {
                // Почати таймер тільки якщо кадри були успішно завантажені
                timer.Start();

            }
            else
            {
                MessageBox.Show("Не вдалося завантажити кадри анімації.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void LoadFrames()
        {
            // Завантажити всі картинки (змініть шляхи до ваших картинок)
            girl = new BitmapImage[8]; // Наприклад, 8 кадрів анімації
            skate = new BitmapImage[6]; // Наприклад, 8 кадрів анімації

            try
            {
                for (int i = 0; i < girl.Length; i++)
                {
                    girl[i] = new BitmapImage(new Uri($"C:\\Users\\home\\Desktop\\3_курс\\ПЗ\\postcard\\postcard\\images\\girl{i + 1}.png"));
                }
                for (int i = 0; i < skate.Length; i++)
                {
                    skate[i] = new BitmapImage(new Uri($"C:\\Users\\home\\Desktop\\3_курс\\ПЗ\\postcard\\postcard\\images\\skate{i + 1}.png"));
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок завантаження кадрів
                girl = null;
                skate = null;
                MessageBox.Show($"Помилка завантаження кадрів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void Timer_Tick(object sender, EventArgs e)
        {
            // Міняємо кадр за допомогою ImageBrush
            currentFrameIndexGirl = (currentFrameIndexGirl + 1) % girl.Length;
            animationImageBrush1.Source = girl[currentFrameIndexGirl];
            currentFrameIndexSkate = (currentFrameIndexSkate + 1) % skate.Length;
            animationImageBrush2.Source = skate[currentFrameIndexSkate];
        }

        private void boxImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            timerBox = new DispatcherTimer();
            timerBox.Interval = TimeSpan.FromSeconds(0.3);
            timerBox.Tick += TimerBox;

            LoadFramesBox();

            if ((box != null && box.Length > 0))
            {
                // Почати таймер тільки якщо кадри були успішно завантажені
                timerBox.Start();

            }
            else
            {
                MessageBox.Show("Не вдалося завантажити кадри анімації.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void LoadFramesBox()
        {
            box = new BitmapImage[6]; // Наприклад, 8 кадрів анімації

            try
            {
                for (int i = 0; i < box.Length; i++)
                {
                    box[i] = new BitmapImage(new Uri($"C:\\Users\\home\\Desktop\\3_курс\\ПЗ\\postcard\\postcard\\images\\box{i + 1}.png"));
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок завантаження кадрів
                box = null;
                MessageBox.Show($"Помилка завантаження кадрів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TimerBox(object sender, EventArgs e)
        {
            // Міняємо кадр за допомогою ImageBrush
            currentFrameIndexBox = (currentFrameIndexBox + 1) % box.Length;
            boxImage.Source = box[currentFrameIndexBox];

            if (currentFrameIndexBox == 0)
            {
                repeatCount++;

                if (repeatCount >= 1)
                {
                    // Зупинити таймер після виконання бажаної кількості повторів
                    timerBox.Stop();
                    boxImage.Source = box[5];
                    AnimateParachute();
                }
            }

        }

        private void AnimateParachute()
        {
            DoubleAnimationUsingKeyFrames verticalAnimation = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames horizontalAnimation = new DoubleAnimationUsingKeyFrames();

            // Анімація для одночасного руху вгору та вперед
            verticalAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(100, TimeSpan.FromSeconds(2))); // Вгору на 100 пікселів за 2 секунди
            horizontalAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(100, TimeSpan.FromSeconds(2))); // Вперед на 100 пікселів за 2 секунди

            // Анімація для руху вниз та вперед
            verticalAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(200, TimeSpan.FromSeconds(4))); // Вниз на 100 пікселів за 2 секунди
            horizontalAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(200, TimeSpan.FromSeconds(4))); // Вперед на 100 пікселів за 2 секунди

            TranslateTransform translateTransform = new TranslateTransform();
            bear.RenderTransform = translateTransform;

            Storyboard.SetTarget(verticalAnimation, translateTransform);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(TranslateTransform.YProperty));

            Storyboard.SetTarget(horizontalAnimation, translateTransform);
            Storyboard.SetTargetProperty(horizontalAnimation, new PropertyPath(TranslateTransform.XProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(verticalAnimation);
            storyboard.Children.Add(horizontalAnimation);

            storyboard.Begin();
        }

    }
}
