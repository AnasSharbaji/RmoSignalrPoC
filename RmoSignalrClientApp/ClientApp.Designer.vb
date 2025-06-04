<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ClientApp
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        lstNotifications = New ListBox()
        lblUpdates = New Label()
        btnLoadHistory = New Button()
        pnlStatus = New Panel()
        lblStatusText = New Label()
        SuspendLayout()
        ' 
        ' lstNotifications
        ' 
        lstNotifications.FormattingEnabled = True
        lstNotifications.Location = New Point(12, 41)
        lstNotifications.Name = "lstNotifications"
        lstNotifications.Size = New Size(580, 139)
        lstNotifications.TabIndex = 0
        ' 
        ' lblUpdates
        ' 
        lblUpdates.AutoSize = True
        lblUpdates.Location = New Point(532, 20)
        lblUpdates.Name = "lblUpdates"
        lblUpdates.Size = New Size(50, 15)
        lblUpdates.TabIndex = 1
        lblUpdates.Text = "Updates"
        ' 
        ' btnLoadHistory
        ' 
        btnLoadHistory.Location = New Point(214, 186)
        btnLoadHistory.Name = "btnLoadHistory"
        btnLoadHistory.Size = New Size(163, 47)
        btnLoadHistory.TabIndex = 2
        btnLoadHistory.Text = "Load History"
        btnLoadHistory.UseVisualStyleBackColor = True
        ' 
        ' pnlStatus
        ' 
        pnlStatus.BackColor = Color.Red
        pnlStatus.Location = New Point(12, 20)
        pnlStatus.Name = "pnlStatus"
        pnlStatus.Size = New Size(20, 20)
        pnlStatus.TabIndex = 3
        ' 
        ' lblStatusText
        ' 
        lblStatusText.AutoSize = True
        lblStatusText.Location = New Point(38, 23)
        lblStatusText.Name = "lblStatusText"
        lblStatusText.Size = New Size(39, 15)
        lblStatusText.TabIndex = 4
        lblStatusText.Text = "Status"
        ' 
        ' ClientApp
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(964, 482)
        Controls.Add(lblStatusText)
        Controls.Add(pnlStatus)
        Controls.Add(btnLoadHistory)
        Controls.Add(lblUpdates)
        Controls.Add(lstNotifications)
        Name = "ClientApp"
        Text = "ClientApp"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lstNotifications As ListBox
    Friend WithEvents lblUpdates As Label
    Friend WithEvents btnLoadHistory As Button
    Friend WithEvents pnlStatus As Panel
    Friend WithEvents lblStatusText As Label

End Class
