using AlgimedWPFApp.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;

namespace AlgimedWPFApp.ViewModels
{
    class DataViewModel : BaseViewModel
    {
        private readonly DBContext _context;

        public DataViewModel(DBContext context)
        {
            _context = context;
            ImportDataCommand = new RelayCommand(async (a) => await ImportFromExcel());
            LoadDataCommand = new RelayCommand(async (a) => await LoadData());
            EditItemCommand = new RelayCommand(async (a) => await EditItem());
            DeleteItemCommand = new RelayCommand(async (a) => await DeleteItem());
        }

        public RelayCommand ImportDataCommand { get; }
        public RelayCommand LoadDataCommand { get; }
        public RelayCommand EditItemCommand { get; }
        public RelayCommand DeleteItemCommand { get; }

        private ObservableCollection<Mode> _modes;
        public ObservableCollection<Mode> Modes
        {
            get => _modes;
            set
            {
                _modes = value;
                OnPropertyChanged(nameof(Modes));
            }
        }

        private ObservableCollection<Step> _steps;
        public ObservableCollection<Step> Steps
        {
            get => _steps;
            set
            {
                _steps = value;
                OnPropertyChanged(nameof(Steps));
            }
        }

        private Mode _selectedMode;
        public Mode SelectedMode
        {
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                OnPropertyChanged(nameof(SelectedMode));
            }
        }

        private Step _selectedStep;
        public Step SelectedStep
        {
            get => _selectedStep;
            set
            {
                _selectedStep = value;
                OnPropertyChanged(nameof(SelectedStep));
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        // Удаление обьекта (вне зависимости от типа)
        private async Task DeleteItem()
        {
            if (SelectedMode != null)
            {
                var selectedMode = SelectedMode;
                if (selectedMode != null)
                {
                    _context.Modes.Remove(selectedMode);
                    Modes.Remove(selectedMode);
                }
            }
            else if (SelectedStep != null)
            {
                var selectedStep = SelectedStep;
                if (selectedStep != null)
                {
                    _context.Steps.Remove(selectedStep);
                    Steps.Remove(selectedStep);
                }
            }
            await _context.SaveChangesAsync();
        }

        // Редактирование обьекта (вне зависимости от типа)
        private async Task EditItem()
        {
            if (SelectedMode != null || SelectedStep != null)
            {
                await _context.SaveChangesAsync();
            }
        }

        // Загрузка данных из БД
        private async Task LoadData()
        {
            Modes = new ObservableCollection<Mode>(await _context.Modes.Include(m => m.Steps).ToListAsync());
            Steps = new ObservableCollection<Step>(await _context.Steps.ToListAsync());
        }

        // Импорт данных из excel-файла в БД (с последующей загрузкой)
        public async Task ImportFromExcel()
        {
            // Проверка валидности введённого пути к файлу
            if (File.Exists(FilePath)
                && (Path.GetExtension(FilePath).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                || Path.GetExtension(FilePath).Equals(".xls", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    using (var workbook = new XLWorkbook(FilePath))
                    {
                        var modesSheet = workbook.Worksheet(1);
                        var stepsSheet = workbook.Worksheet(2);

                        List<Mode> modesToAdd = new();
                        List<Step> stepsToAdd = new();

                        // Чтение данных и добавление в контекст
                        foreach (var row in modesSheet.RowsUsed().Skip(1))
                        {
                            var mode = new Mode
                            {
                                ID = row.Cell(1).GetValue<int>(),
                                Name = row.Cell(2).GetString(),
                                MaxBottleNumber = row.Cell(3).GetValue<int>(),
                                MaxUsedTips = row.Cell(4).GetValue<int>()
                            };
                            // Попытка добавить запись в базу (если уже есть запись с таким ID, метод прекращает свою работу и все данные из файла не добавляются)
                            if (await _context.Modes.AnyAsync(x => x.ID == mode.ID))
                            {
                                MessageBox.Show($"Mode with ID {mode.ID} already exists in database, please fix excel file");
                                return;
                            }
                            modesToAdd.Add(mode);
                        }

                        foreach (var row in stepsSheet.RowsUsed().Skip(1))
                        {
                            var step = new Step
                            {
                                ID = row.Cell(1).GetValue<int>(),
                                ModeId = row.Cell(2).GetValue<int>(),
                                Timer = TimeSpan.FromSeconds(row.Cell(3).GetValue<double>()),
                                Destination = row.Cell(4).GetString(),
                                Speed = row.Cell(5).GetValue<int>(),
                                Type = row.Cell(6).GetString(),
                                Volume = row.Cell(7).GetValue<double>()
                            };
                            // Попытка добавить запись в базу (если уже есть запись с таким ID, метод прекращает свою работу и все данные из файла не добавляются)
                            if (await _context.Steps.AnyAsync(x => x.ID == step.ID))
                            {
                                MessageBox.Show($"Step with ID {step.ID} already exists in database, please fix excel file");
                                return;
                            }
                            stepsToAdd.Add(step);
                        }

                        await _context.Modes.AddRangeAsync(modesToAdd);
                        await _context.Steps.AddRangeAsync(stepsToAdd);

                        await _context.SaveChangesAsync();
                    }
                    await LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured when trying to import data from Excel file: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Filepath is invalid");
            }
        }
    }
}
