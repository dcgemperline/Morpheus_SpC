# Morpheus Spectral Counter (MSpC) Tutorial
------------------------------
## Requirements
* 64 bit Windows Installation: For Example Microsoft Windows 7 64 bit, Windows 8 and 8.1 64bit, or Windows 10 64bit
	* This is due to requirements from vendor libraries to process Thermo .raw files directly in this tutorial

* Download and install [proteowizard](http://proteowizard.sourceforge.net/downloads.shtml) 
	* Installation of proteowizard correctly installs the latest Thermo Vendor .dll's to read .raw files directly

* Download pre-compiled binaries of revision 151 of the [Morpheus Mass Spectrometry Search Engine (Thermo Version)](https://github.com/dcgemperline/Morpheus_SpC/releases/download/v1.0/Morpheus.Binaries.revision.151.zip), referred from here on out as just Morpheus
* Download and install [MSpC](https://github.com/dcgemperline/Morpheus_SpC/releases/download/v1.0/MSpC_v1.0.zip)
	* MSpC depends on the latest .NET Runtime that will be installed with MSpC installer if it is not already installed
* Download the data files from the PRIDE proteomics data repository using the following ID - PXD003002  [Data Files](http://www.ebi.ac.uk/pride/archive/)

------------------------------
## Data Analysis Starting from .raw files

1. Once all the requisite software is installed and the Data Files are downloaded you are ready to begin.
2. Unzip **Morpheus Binaries revision 151.zip** into your desired location and start **Morpheus (Thermo).exe** from the Morpheus (Thermo) folder.
3. Once Morpheus starts, verify at the top you are using **Morpheus (Thermo) revision 151**.
4. Add the following .raw files to Morpheus from the Raw Files folder contained in the Data Files folder (multiple files can be selected at once).
   * 04_28_15_A_rep1_c1.raw
   * 04_28_15_A_rep2_c1.raw
   * 04_28_15_A_rep3_c1.raw
   * 04_28_15_B_rep1_c1.raw
   * 04_28_15_B_rep2_c1.raw
   * 04_28_15_B_rep3_c1.raw
   * 04_28_15_C_rep1_c1.raw
   * 04_28_15_C_rep2_c1.raw
   * 04_28_15_C_rep3_c1.raw
   * 04_28_15_D_rep1_c1.raw
   * 04_28_15_D_rep2_c1.raw
   * 04_28_15_D_rep3_c1.raw
5. Side Note on Fractionation: If you would like Morpheus, and thus MSpC to output summaries for a set of data by adding up all of the spectra identified in that dataset(as the case may be for fractionation data), place the files you would like to be summarized in seperate folders, such as SampleSet1, Sampleset2, and Morphues will generate protein_groups.tsv and PSMs.tsv specifically for each sampleset. Later in MSpC after selecting the summary.tsv file these will show up as SampleSet1\*, SampleSet2\*, etc.
6. Browse for the fasta file contained in the Data Files Folder **uniprot_k12_e_coli_contams_plus_proteasome_and_interactors.fasta**.
7. Verify that Create Target-Decoy Database On The Fly is checked.
8. Browse for an Output Folder. In this tutorial select browse, leave the default Desktop location highlighted and press Make a New Folder and rename it Morpheus Analysis. Press OK. You should now have a folder on the desktop called Morpheus Analysis, and Morpheus should say you are outputting data to the following output folder, where _NAME_ is your username on your machine.
	* C:\Users\\_NAME_\Desktop\Morpheus Analysis
8. Set the Maximum threads to 2, for a dual core processor, 4 for a quad core processor, 8 for a quad core processor with hyperthreading, and 8 for an 8 core processor. These are reasonable values that will give you decent performance with Morpheus.
9. Press Search, and the progress bar will indicate search progress for each raw file. On an Intel Core i7 2700K with Morpheus set to use 8 threads, this takes approximately 4.5 minutes.
10. Close Morpheus if so desired.
11. On completion of the search open MSpC.
12. The indicators in Summary Directory Contains Valid Input should be red.
13. Press the Select the Summary File button and navigate to the summary.tsv file contained in the Morpheus Analysis folder on the Desktop.
    * C:\Users\\_NAME_\Desktop\Morpheus Analysis\summary.tsv
14. The indicators in Summary Directory Contains Valid Input should now read green indicating that the Morpheus Analysis folder contains all the necessary files from Morpheus to run. (The files requried to be in the same directory as summary.tsv are protein_groups.tsv and PSMs.tsv. This will automatically occur if you output all of your Morpheus output into a single output folder.)
15. (Optionally) Select the provided whitelist file contained in the Data Files folder that you downloaded to simplify the output.
16. Select your desired PSM FDR and Protein FDR (although the default options of 1% are good default options).
17. Check or uncheck boxes of the desired calculations (NSAF, dNSAF, uNSAF)
18. (Optionally) Select an Output Directory, otherwise MSpC defaults to the current directory of summary.tsv as an output directory.
    * Let's press Select Output Directory then Make a New Folder here called MSpC Output, press OK.
19. Press Summarize Data, and the progress bar will indicate the progress of MSpC. This should take ~ 30 seconds or less
20. The MSpC Output folder will then contain NSAF summaries for each individual .raw file, as well as a summary for all .raw files analyzed. These files are in the tab delimited output format .tsv

------------------------------
## Data Analysis Starting from previously searched Morpheus Data
1. It is strongly suggested, that you start with the Data Analysis from .raw files tutorial first, and then refer back to the tutorial for previously searched Morpheus data if necessary
2. Open MSpC
3. Select a Morpheus summary.tsv file in a directory that contains the entire output from Morpheus.
4. If all output is there, the indicators in Summary Directory Contains Valid Input should now read green indicating that the folder contains all the necessary files from Morpheus to run MSpC.
5. (Optionally) Select the provided whitelist file contained in the Data Files folder that you downloaded to simplify the output.
6. Select your desired PSM FDR and Protein FDR (although the default options of 1% are good default options).
7. Check or uncheck boxes of the desired calculations (NSAF, dNSAF, uNSAF)
8. (Optionally) Select an Output Directory, otherwise MSpC defaults to the current directory of summary.tsv as an output directory.
    * Let's press Select Output Directory then Make a New Folder here called MSpC Output, press OK.
9. Press Summarize Data, and the progress bar will indicate the progress of MSpC. This should take ~ 30 seconds or less
10. The MSpC Output folder will then contain NSAF summaries for each individual  .raw file, as well as a summary for all .raw files analyzed. These files are in the tab delimited output format .tsv
